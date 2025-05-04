using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using Server.Shared.DTOs;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Client.ViewModels.MenuControls
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly DashboardApiService _dashboardApiService = new(App.SharedHttpClient);

        public DashboardViewModel()
        {
        }

        [ObservableProperty]
        private ObservableCollection<MaterialCurrentStockDto> materials = new();

        [ObservableProperty]
        private MaterialCurrentStockDto? selectedMaterial;

        [ObservableProperty]
        private ISeries[] stockSeries = Array.Empty<ISeries>();

        [ObservableProperty]
        private Axis[] xAxes = Array.Empty<Axis>();

        [ObservableProperty]
        private Axis[] yAxes = Array.Empty<Axis>();

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            var materialsResult = await _dashboardApiService.GetCurrentStocksAsync();
            Materials = new ObservableCollection<MaterialCurrentStockDto>(materialsResult);

            // Якщо є хоч один матеріал - вибираємо його
            SelectedMaterial = Materials.FirstOrDefault();

            if (SelectedMaterial != null)
            {
                await OnMaterialSelectedAsync();
            }
        }


        [RelayCommand]
        public async Task OnMaterialSelectedAsync()
        {
            if (SelectedMaterial == null)
                return;
        
                    var history = await _dashboardApiService.GetStockHistoryAsync(SelectedMaterial.MaterialItemId);
        
                    var groupedHistory = history
            .GroupBy(h => h.Date.Date)
            .Select(g => new
            {
                Date = g.Key,
                Stock = g.Last().Stock
            })
            .OrderBy(x => x.Date)
            .ToList();
        
                    var stockPoints = groupedHistory
                        .Select(h => new ObservablePoint(h.Date.ToOADate(), (double)h.Stock))
                        .ToArray();
        
                    var minStock = (double)SelectedMaterial.MinimumStock;
        
                    var minStockPoints = groupedHistory
                        .Select(h => new ObservablePoint(h.Date.ToOADate(), minStock))
                        .ToArray();

            StockSeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = stockPoints,
                    Fill = null, 
                    Stroke = new SolidColorPaint(SkiaSharp.SKColors.Blue, 3),
                    GeometryFill = new SolidColorPaint(SKColors.Blue),
                    GeometryStroke = new SolidColorPaint(SKColors.White),
                    LineSmoothness = 0
                },
                new LineSeries<ObservablePoint>
                {
                    Values = minStockPoints,
                    Fill = null,
                   Stroke = new SolidColorPaint(SkiaSharp.SKColors.Red, 2),
                   LineSmoothness = 0,
                   GeometrySize = 0
        
                }
            };
        
                    XAxes = new Axis[]
                {
            new Axis
            {
                Labeler = value => DateTime.FromOADate(value).ToString("d MMM"),
                UnitWidth = TimeSpan.FromDays(1).TotalDays,
                LabelsRotation = 15,
                MinStep = TimeSpan.FromDays(1).TotalDays 
            }
                };
        
        
                    YAxes = new Axis[]
            {
                new Axis()
            };
        }


        partial void OnSelectedMaterialChanged(MaterialCurrentStockDto? value)
        {
            if (value != null)
            {
                _ = OnMaterialSelectedAsync();
            }
        }
    }
}
