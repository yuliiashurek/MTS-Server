using Client.Models;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class PrintLabelForm : HandyControl.Controls.Window
    {
        public PrintSettings Result { get; private set; }

        public PrintLabelForm()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(CopiesTextBox.Text, out int copies) || copies <= 0)
            {
                MessageBox.Show("Введіть коректну кількість копій.");
                return;
            }

            var printMode = (PrintMode)((ComboBoxItem)PrintModeComboBox.SelectedItem).Tag;

            Result = new PrintSettings
            {
                PrintMode = printMode,
                Copies = copies
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
