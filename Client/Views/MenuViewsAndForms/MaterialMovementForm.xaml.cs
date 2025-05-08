using Client.Models;
using Client.Services.ApiServices;
using Server.Shared.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class MaterialMovementForm : Window
    {
        public MaterialMovement Result { get; private set; }

        private readonly ObservableCollection<MaterialItem> _materials;
        private readonly ObservableCollection<Warehouse> _warehouses;
        private readonly RecipientApiService _recipientApi = new();

        private class MovementTypeItem
        {
            public string Name { get; set; }
            public int Type { get; set; }
        }

        public MaterialMovementForm(MaterialMovement? movement, ObservableCollection<MaterialItem> materials, ObservableCollection<Warehouse> warehouses)
        {
            InitializeComponent();

            _materials = materials;
            _warehouses = warehouses;

            MaterialComboBox.ItemsSource = _materials;
            WarehouseComboBox.ItemsSource = _warehouses;

            MovementTypeComboBox.ItemsSource = new[]
            {
                new MovementTypeItem { Name = "Прийом (IN)", Type = 0 },
                new MovementTypeItem { Name = "Витрата (OUT)", Type = 1 }
            };
            MovementTypeComboBox.SelectedIndex = 0;
            MovementTypeComboBox.SelectionChanged += (s, e) => UpdateVisibility();

            Loaded += async (_, _) => await LoadAsync(movement);
        }

        private async Task LoadAsync(MaterialMovement? movement)
        {
            UpdateVisibility();

            if (movement == null)
            {
                MovementDatePicker.SelectedDate = DateTime.Now;
                return;
            }

            MaterialComboBox.SelectedItem = _materials.FirstOrDefault(m => m.Id == movement.MaterialItemId);
            WarehouseComboBox.SelectedItem = _warehouses.FirstOrDefault(w => w.Id == movement.WarehouseId);
            MovementTypeComboBox.SelectedItem = ((MovementTypeItem[])MovementTypeComboBox.ItemsSource)
                .FirstOrDefault(x => x.Type == movement.MovementType);

            QuantityTextBox.Text = movement.Quantity.ToString();
            PriceTextBox.Text = movement.PricePerUnit.ToString();
            MovementDatePicker.SelectedDate = movement.MovementDate;
            ExpirationDatePicker.SelectedDate = movement.ExpirationDate;

            if (movement.MovementType == 1 && movement.RecipientId.HasValue)
            {
                try
                {
                    var recipient = await _recipientApi.GetByIdAsync(movement.RecipientId.Value);
                    if (recipient != null)
                    {
                        RecipientNameTextBox.Text = recipient.Name;
                        RecipientEdrpouTextBox.Text = recipient.Edrpou;
                        RecipientAddressTextBox.Text = recipient.Address;
                        RecipientContactTextBox.Text = recipient.ContactPerson;

                        RecipientEdrpouTextBox.IsEnabled = false;
                        RecipientAddressTextBox.IsEnabled = false;
                        RecipientContactTextBox.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не вдалося завантажити отримувача: " + ex.Message);
                }
            }
        }

        private void UpdateVisibility()
        {
            WarehouseRow.Visibility = Visibility.Visible;
            PriceRow.Visibility = Visibility.Visible;
            ExpirationRow.Visibility = MovementTypeComboBox.SelectedItem is MovementTypeItem selected && selected.Type == 1
                ? Visibility.Collapsed
                : Visibility.Visible;

            RecipientPanel.Visibility = MovementTypeComboBox.SelectedItem is MovementTypeItem m && m.Type == 1
                ? Visibility.Visible
                : Visibility.Collapsed;
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
            }
            else
            {
                RecipientEdrpouTextBox.Text = "";
                RecipientAddressTextBox.Text = "";
                RecipientContactTextBox.Text = "";

                RecipientEdrpouTextBox.IsEnabled = true;
                RecipientAddressTextBox.IsEnabled = true;
                RecipientContactTextBox.IsEnabled = true;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem is not MaterialItem selectedMaterial ||
                !decimal.TryParse(QuantityTextBox.Text, out var quantity))
            {
                MessageBox.Show("Будь ласка, заповніть обов’язкові поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedType = (MovementTypeItem)MovementTypeComboBox.SelectedItem;
            bool isOut = selectedType.Type == 1;
            Guid? recipientId = null;

            if (isOut)
            {
                if (string.IsNullOrWhiteSpace(RecipientNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(RecipientEdrpouTextBox.Text) ||
                    string.IsNullOrWhiteSpace(RecipientAddressTextBox.Text))
                {
                    MessageBox.Show("Будь ласка, заповніть усі поля отримувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existing = await _recipientApi.GetByNameAsync(RecipientNameTextBox.Text);
                if (existing == null)
                {
                    existing = await _recipientApi.CreateAsync(new RecipientDto
                    {
                        Name = RecipientNameTextBox.Text,
                        Edrpou = RecipientEdrpouTextBox.Text,
                        Address = RecipientAddressTextBox.Text,
                        ContactPerson = RecipientContactTextBox.Text
                    });
                }

                recipientId = existing.Id;
            }

            Result = new MaterialMovement
            {
                MaterialItemId = selectedMaterial.Id,
                WarehouseId = ((Warehouse)WarehouseComboBox.SelectedItem)?.Id ?? Guid.Empty,
                MovementType = selectedType.Type,
                Quantity = quantity,
                PricePerUnit = isOut ? 0 : decimal.TryParse(PriceTextBox.Text, out var price) ? price : 0,
                MovementDate = MovementDatePicker.SelectedDate ?? DateTime.Now,
                ExpirationDate = isOut ? null : ExpirationDatePicker.SelectedDate,
                RecipientId = recipientId,
                BarcodeNumber = string.Empty
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
