// ForecastDashboardViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Server.Shared.DTOs;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Client.ViewModels
{
    public partial class ForecastDashboardViewModel : ObservableObject
    {
        private readonly DashboardApiService _dashboardApiService = new(App.SharedHttpClient);

        public ForecastDashboardViewModel()
        {
            LoadForecastAsync();
        }

        [ObservableProperty]
        private ObservableCollection<ForecastDashboardItemDto> items = new();

        [ObservableProperty]
        private ForecastDashboardItemDto? selectedItem;

        [ObservableProperty] private double selectedCurrentStock;
        [ObservableProperty] private double selectedMinStock;
        [ObservableProperty] private double selectedForecast;

        [ObservableProperty] private ISeries[] series = Array.Empty<ISeries>();

        [ObservableProperty] private Axis[] xAxes = Array.Empty<Axis>();

        [ObservableProperty]
        private Axis[] yAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                LabelsPaint = new SolidColorPaint(SKColors.Gray)
            }
        };

        [RelayCommand]
        private async Task LoadForecastAsync()
        {
            var result = await _dashboardApiService.GetForecastAsync();

            foreach (var item in result)
            {
                item.IsCritical = item.CurrentStock <= item.MinStock;
                item.IsWarning = !item.IsCritical && item.CurrentStock <= item.MinStock * 1.1;
                item.IsNormal = !item.IsCritical && !item.IsWarning;
            }

            Items = new ObservableCollection<ForecastDashboardItemDto>(result);
            if (Items.Any())
                SelectedItem = Items.First();
        }

        partial void OnSelectedItemChanged(ForecastDashboardItemDto? value)
        {
            if (value == null) return;

            SelectedCurrentStock = value.CurrentStock;
            SelectedMinStock = value.MinStock;
            SelectedForecast = value.Forecast7Days;

            LoadChartAsync(value.MaterialId);
        }

        private async void LoadChartAsync(Guid materialId)
        {
            var raw = await _dashboardApiService.GetMaterialOutflowHistoryAsync(materialId);

            var grouped = raw
                .GroupBy(x => x.Date.Date)
                .ToDictionary(g => g.Key, g => (double)g.Sum(x => x.OutQuantity));

            var today = DateTime.Today;

            var pastPoints = Enumerable.Range(0, 30)
                .Select(i => today.AddDays(-29 + i))
                .Select(date => (Date: date, Usage: grouped.TryGetValue(date, out var v) ? v : 0))
                .ToList();

            var forecastPoints = new List<(DateTime Date, double Usage)>();
            for (int i = 1; i <= 7; i++)
            {
                var date = today.AddDays(i);
                var usage = GetWeightedForecast(pastPoints, date);
                forecastPoints.Add((date, usage));
            }

            var allDates = pastPoints.Select(p => p.Date).Concat(forecastPoints.Select(p => p.Date)).ToList();

            Series = new ISeries[]
            {
                new ColumnSeries<double?>
                {
                    Name = "Факт",
                    Values = allDates
                        .Select(date => pastPoints.FirstOrDefault(p => p.Date == date).Usage)
                        .Cast<double?>()
                        .ToArray(),
                    Fill = new SolidColorPaint(SKColors.SteelBlue),
                    Stroke = null
                },
                new ColumnSeries<double?>
                {
                    Name = "Прогноз",
                    Values = allDates
                        .Select(date => forecastPoints.FirstOrDefault(p => p.Date == date).Usage)
                        .Cast<double?>()
                        .ToArray(),
                    Fill = new SolidColorPaint(SKColors.Gray.WithAlpha(180)),
                    Stroke = null
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = allDates.Select(d => d.ToString("dd.MM")).ToArray(),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                }
            };
        }

        private double GetWeightedForecast(List<(DateTime Date, double Usage)> history, DateTime targetDate)
        {
            const double alpha = 0.3;

            var ordered = history
                .Where(p => p.Date < targetDate)
                .OrderBy(p => p.Date)
                .ToList();

            if (!ordered.Any()) return 0;

            double smoothed = ordered.First().Usage;

            foreach (var point in ordered.Skip(1))
            {
                smoothed = alpha * point.Usage + (1 - alpha) * smoothed;
            }

            var weekdayMultipliers = new Dictionary<DayOfWeek, double>
            {
                [DayOfWeek.Monday] = 1.0,
                [DayOfWeek.Tuesday] = 1.05,
                [DayOfWeek.Wednesday] = 0.95,
                [DayOfWeek.Thursday] = 1.1,
                [DayOfWeek.Friday] = 1.2,
                [DayOfWeek.Saturday] = 0.6,
                [DayOfWeek.Sunday] = 0.5
            };

            var multiplier = weekdayMultipliers[targetDate.DayOfWeek];
            return Math.Round(smoothed * multiplier, 2);
        }
    }
}
