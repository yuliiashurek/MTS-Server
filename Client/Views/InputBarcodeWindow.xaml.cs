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
using System.Windows.Shapes;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for InputBarcodeWindow.xaml
    /// </summary>
    public partial class InputBarcodeWindow : HandyControl.Controls.Window
    {
        public string Barcode { get; private set; }

        public InputBarcodeWindow()
        {
            InitializeComponent();
            BarcodeTextBox.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Barcode = BarcodeTextBox.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

}
