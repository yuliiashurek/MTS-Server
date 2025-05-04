using Client.Models;
using Server.Shared.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class MaterialMovementForm : Window
    {
        public MaterialMovement Result { get; private set; }

        private readonly ObservableCollection<MaterialItem> _materials;
        private readonly ObservableCollection<Warehouse> _warehouses;

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
            MaterialComboBox.DisplayMemberPath = "Name";

            WarehouseComboBox.ItemsSource = _warehouses;
            WarehouseComboBox.DisplayMemberPath = "Name";

            MovementTypeComboBox.ItemsSource = new[]
            {
                new MovementTypeItem { Name = "Прийом (IN)", Type = 0 },
                new MovementTypeItem { Name = "Витрата (OUT)", Type = 1 }
            };
            MovementTypeComboBox.DisplayMemberPath = "Name";
            MovementTypeComboBox.SelectedIndex = 0;

            if (movement != null)
            {
                MaterialComboBox.SelectedItem = _materials.FirstOrDefault(m => m.Id == movement.MaterialItemId);
                WarehouseComboBox.SelectedItem = _warehouses.FirstOrDefault(w => w.Id == movement.WarehouseId);
                MovementTypeComboBox.SelectedItem = ((MovementTypeItem[])MovementTypeComboBox.ItemsSource)
                    .FirstOrDefault(x => x.Type == movement.MovementType);

                QuantityTextBox.Text = movement.Quantity.ToString();
                PriceTextBox.Text = movement.PricePerUnit.ToString();
                MovementDatePicker.SelectedDate = movement.MovementDate;
                ExpirationDatePicker.SelectedDate = movement.ExpirationDate;
            }
            else
            {
                MovementDatePicker.SelectedDate = DateTime.Now;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem is not MaterialItem selectedMaterial ||
                WarehouseComboBox.SelectedItem is not Warehouse selectedWarehouse ||
                !decimal.TryParse(QuantityTextBox.Text, out var quantity))
            {
                MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля правильно.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var priceParsed = decimal.TryParse(PriceTextBox.Text, out var pricePerUnit);
            var selectedMovementType = (MovementTypeItem)MovementTypeComboBox.SelectedItem;

            Result = new MaterialMovement
            {
                MaterialItemId = selectedMaterial.Id,
                WarehouseId = selectedWarehouse.Id,
                MovementType = selectedMovementType.Type,
                Quantity = quantity,
                PricePerUnit = priceParsed ? pricePerUnit : 0,
                MovementDate = MovementDatePicker.SelectedDate ?? DateTime.Now,
                ExpirationDate = ExpirationDatePicker.SelectedDate,
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
