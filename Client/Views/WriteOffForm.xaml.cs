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

using Client.Services.ApiServices;
using Server.Shared.DTOs;
namespace Client.Views
{
    /// <summary>
    /// Interaction logic for WriteOffForm.xaml
    /// </summary>

    public partial class WriteOffForm : HandyControl.Controls.Window
    {
        private readonly RecipientApiService _recipientApi = new();

        public Guid? SelectedRecipientId { get; private set; }

        public WriteOffForm()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void RecipientNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var name = RecipientNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(name)) return;

            var existing = await _recipientApi.GetByNameAsync(name);
            if (existing != null)
            {
                RecipientEdrpouTextBox.Text = existing.Edrpou;
                RecipientAddressTextBox.Text = existing.Address;
                RecipientContactTextBox.Text = existing.ContactPerson;

                RecipientEdrpouTextBox.IsEnabled = false;
                RecipientAddressTextBox.IsEnabled = false;
                RecipientContactTextBox.IsEnabled = false;

                SelectedRecipientId = existing.Id;
            }
            else
            {
                RecipientEdrpouTextBox.Text = "";
                RecipientAddressTextBox.Text = "";
                RecipientContactTextBox.Text = "";

                RecipientEdrpouTextBox.IsEnabled = true;
                RecipientAddressTextBox.IsEnabled = true;
                RecipientContactTextBox.IsEnabled = true;

                SelectedRecipientId = null;
            }
        }
    }


}
