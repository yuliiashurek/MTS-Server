using Client.Models;
using Client.Services.ApiServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public partial class TransferReportViewModel : ObservableObject
    {
        private readonly ReportsApiService _reportsApiService;
        private readonly RecipientApiService _recipientApiService;

        public TransferReportViewModel()
        {
            _reportsApiService = new ReportsApiService();
            _recipientApiService = new RecipientApiService();
            LoadRecipientsCommand.Execute(null);
        }

        [ObservableProperty]
        private ObservableCollection<Recipient> recipients = new();

        [ObservableProperty]
        private Recipient? selectedRecipient;

        [ObservableProperty]
        private DateTime dateFrom = DateTime.Today.AddDays(-7);

        [ObservableProperty]
        private DateTime dateTo = DateTime.Today;

        [ObservableProperty]
        private string? contractNumber;

        [ObservableProperty]
        private string? htmlPreview;


        [RelayCommand]
        private async Task LoadRecipientsAsync()
        {
            var result = await _recipientApiService.GetRecipientsAsync();
            Recipients = new ObservableCollection<Recipient>(result);
        }

        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            if (SelectedRecipient == null) return;

            var html = await _reportsApiService.GetTransferActHtmlPreviewAsync(
                SelectedRecipient.Id, DateFrom, DateTo, ContractNumber
            );

            if (!string.IsNullOrWhiteSpace(html))
            {
                HtmlPreview = html;
            }
        }

        [RelayCommand]
        private async Task DownloadReportAsync()
        {
            if (SelectedRecipient == null) return;

            var pdf = await _reportsApiService.GenerateTransferActAsync(
                SelectedRecipient.Id, DateFrom, DateTo, ContractNumber
            );

            if (pdf != null)
            {
                var dialog = new SaveFileDialog
                {
                    FileName = "TransferAct.pdf",
                    Filter = "PDF files (*.pdf)|*.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(dialog.FileName, pdf);
                }
            }
        }
    }
}
