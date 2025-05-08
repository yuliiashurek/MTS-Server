using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for TransferReportView.xaml
    /// </summary>
    public partial class TransferReportView : UserControl
    {
        public TransferReportView()
        {
            InitializeComponent();

            var vm = (TransferReportViewModel)DataContext;
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.HtmlPreview) && !string.IsNullOrWhiteSpace(vm.HtmlPreview))
                {
                    BrowserPreview.NavigateToString(vm.HtmlPreview);
                }
            };
        }
    }
}
