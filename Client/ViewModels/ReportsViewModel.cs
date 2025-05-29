using Client.Services.ApiServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Server.Shared.DTOs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public partial class ReportsViewModel : ObservableObject
    {
        private readonly ReportsApiService _reportsApiService;
        private readonly SupplierApiService _supplierApiService;

        public ReportsViewModel()
        {
            _reportsApiService = new ReportsApiService();
            _supplierApiService = new SupplierApiService(App.SharedHttpClient);
            LoadSuppliersCommand.Execute(null);
            LoadEmptyReportCommand.Execute(null);
        }

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers = new();

        [ObservableProperty]
        private Supplier? selectedSupplier;

        [ObservableProperty]
        private DateTime dateFrom = DateTime.Today.AddDays(-7);

        [ObservableProperty]
        private DateTime dateTo = DateTime.Today;

        [ObservableProperty]
        private string? contractNumber;

        [ObservableProperty]
        private string? htmlPreview;

        [ObservableProperty]
        private string? pdfFilePath;

        [RelayCommand]
        private async Task LoadSuppliersAsync()
        {
            var result = await _supplierApiService.GetSuppliersAsync();
            Suppliers = new ObservableCollection<Supplier>(result);
        }

        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            if (SelectedSupplier == null) return;

            var html = await _reportsApiService.GetAcceptanceActHtmlPreviewAsync(
                SelectedSupplier.Id, DateFrom, DateTo, ContractNumber
            );

            if (!string.IsNullOrWhiteSpace(html))
            {
                HtmlPreview = html;
            }
        }

        [RelayCommand]
        private async Task DownloadReportAsync()
        {
            if (SelectedSupplier == null) return;
            await GenerateReportAsync();
            var pdfBytes = await _reportsApiService.GenerateAcceptanceActAsync(
                SelectedSupplier.Id, DateFrom, DateTo, ContractNumber
            );

            if (pdfBytes != null)
            {
                var dialog = new SaveFileDialog
                {
                    FileName = "AcceptanceAct.pdf",
                    Filter = "PDF files (*.pdf)|*.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(dialog.FileName, pdfBytes);
                }
            }
        }

        [RelayCommand]
        private async Task LoadEmptyReportAsync()
        {
            var html = await _reportsApiService.GetEmptyAcceptanceActHtmlAsync();
            if (!string.IsNullOrWhiteSpace(html))
            {
                HtmlPreview = html;
            }
        }

    }
}
