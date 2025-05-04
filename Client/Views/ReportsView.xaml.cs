using Client.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
            ((ReportsViewModel)DataContext).PropertyChanged += ReportsView_PropertyChanged;
        }

        private void ReportsView_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var vm = (ReportsViewModel)DataContext;

            if (e.PropertyName == nameof(ReportsViewModel.HtmlPreview))
            {
                if (!string.IsNullOrEmpty(vm.HtmlPreview))
                {
                    var tempPath = Path.Combine(Path.GetTempPath(), $"preview_{Guid.NewGuid()}.html");
                    File.WriteAllText(tempPath, vm.HtmlPreview);
                    BrowserPreview.Navigate(new Uri(tempPath));
                }
            }
        }
    }
}
