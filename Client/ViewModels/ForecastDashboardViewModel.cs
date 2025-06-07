using Client.Helpers.Others;
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

            var groupedDaily = raw
                .GroupBy(x => x.Date.Date)
                .ToDictionary(g => g.Key, g => (double)g.Sum(x => x.OutQuantity));

            var thisMonday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            var startWeek = thisMonday.AddDays(-7 * 3); // 3 тижні назад

            var weeks = new List<DateTime>();
            for (int i = 0; i < 6; i++) // 3 факт + 1 поточний + 2 прогноз
                weeks.Add(startWeek.AddDays(i * 7));

            var actualValues = new List<double?>();
            var forecastValues = new List<double?>();

            var weeklyUsage = new List<double>();

            foreach (var weekStart in weeks)
            {
                var weekdays = Enumerable.Range(0, 5) // ПН–ПТ
                    .Select(i => weekStart.AddDays(i))
                    .ToList();

                var usage = weekdays.Sum(d => groupedDaily.TryGetValue(d, out var v) ? v : 0);
                weeklyUsage.Add(usage);
            }

            // 3 тижні факт
            for (int i = 0; i < 3; i++)
            {
                actualValues.Add(weeklyUsage[i]);
                forecastValues.Add(null);
            }

            // поточний тиждень
            var thisWeekFact = weeklyUsage[3];
            var thisWeekForecast = GetTrendForecast(weeklyUsage.Take(3).ToList());
            actualValues.Add(thisWeekFact);
            forecastValues.Add(thisWeekForecast);

            // фактичне та прогнозоване значення для поточного тижня
            double fact = weeklyUsage[3];
            double forecast = GetTrendForecast(weeklyUsage.Take(3).ToList());

            double weightForecast;
            double weightFact;

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    weightForecast = 1.0;
                    weightFact = 0.0;
                    break;
                case DayOfWeek.Tuesday:
                    weightForecast = 0.75;
                    weightFact = 0.25;
                    break;
                case DayOfWeek.Wednesday:
                    weightForecast = 0.5;
                    weightFact = 0.5;
                    break;
                case DayOfWeek.Thursday:
                    weightForecast = 0.25;
                    weightFact = 0.75;
                    break;
                case DayOfWeek.Friday:
                default:
                    weightForecast = 0.0;
                    weightFact = 1.0;
                    break;
            }

            var weightedCurrentWeek = Math.Round(
                forecast * weightForecast + fact * weightFact,
                2);

            // прогноз на 2 наступні тижні
            var baseForForecast = weeklyUsage.Take(3).ToList();
            baseForForecast.Add(weightedCurrentWeek);

            var next1 = GetTrendForecast(baseForForecast);
            baseForForecast.Add(next1);

            var next2 = GetTrendForecast(baseForForecast);

            actualValues.Add(null);
            forecastValues.Add(next1);
            actualValues.Add(null);
            forecastValues.Add(next2);

            // графік
            Series = new ISeries[]
            {
                new ColumnSeries<double?>
                {
                    Name = "Факт",
                    Values = actualValues.ToArray(),
                    Fill = new SolidColorPaint(SKColors.SteelBlue),
                    Stroke = null
                },
                new ColumnSeries<double?>
                {
                    Name = "Прогноз",
                    Values = forecastValues.ToArray(),
                    Fill = new SolidColorPaint(SKColors.Gray.WithAlpha(180)),
                    Stroke = null
                }
                    };

                XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = weeks.Select(w => $"{w.AddDays(2):dd.MM}").ToArray(), // середа (центр ПН–ПТ)
                        LabelsPaint = new SolidColorPaint(SKColors.Black),
                        SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
                    }
                };
            }


        private double GetTrendForecast(List<double> weeklyValues)
        {
            if (weeklyValues.Count < 2)
                return weeklyValues.FirstOrDefault();

            int n = weeklyValues.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                double x = i + 1;
                double y = weeklyValues[i];

                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = (sumY - slope * sumX) / n;

            double forecast = slope * (n + 1) + intercept;

            return Math.Round(Math.Max(forecast, 0), 2);
        }

    }
}
