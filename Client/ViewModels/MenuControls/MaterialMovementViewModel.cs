using Client.Models;
using Client.Services;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public partial class MaterialMovementViewModel : ObservableObject
    {
        private readonly MaterialMovementApiService _apiService = new(App.SharedHttpClient);
        private readonly MaterialItemApiService _materialItemApiService = new(App.SharedHttpClient);
        private readonly WarehouseApiService _warehouseApiService = new(App.SharedHttpClient);
        private readonly SupplierApiService _supplierApiService = new(App.SharedHttpClient);
        private readonly CategoryApiService _categoryApiService = new(App.SharedHttpClient);
        private readonly MeasurementUnitApiService _measurementUnitApiService = new(App.SharedHttpClient);

        [ObservableProperty]
        private ObservableCollection<MaterialMovement> materialMovements = new();

        [ObservableProperty]
        private ObservableCollection<MaterialItem> materialItems = new();

        [ObservableProperty]
        private ObservableCollection<Warehouse> warehouses = new();

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers = new();

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private ObservableCollection<MeasurementUnit> measurementUnits = new();

        [ObservableProperty]
        private MaterialMovement? selectedMaterialMovement;

        public MaterialItem? SelectedMaterialItem =>
            SelectedMaterialMovement == null ? null :
            MaterialItems.FirstOrDefault(x => x.Id == SelectedMaterialMovement.MaterialItemId);

        public Supplier? SelectedSupplier =>
            SelectedMaterialItem == null ? null :
            Suppliers.FirstOrDefault(x => x.Id == SelectedMaterialItem.SupplierId);

        public Warehouse? SelectedWarehouse =>
            SelectedMaterialMovement == null ? null :
            Warehouses.FirstOrDefault(x => x.Id == SelectedMaterialMovement.WarehouseId);

        public Category? SelectedCategory =>
            SelectedMaterialItem == null ? null :
            Categories.FirstOrDefault(x => x.Id == SelectedMaterialItem.CategoryId);

        public MeasurementUnit? SelectedMeasurementUnit =>
            SelectedMaterialItem == null ? null :
            MeasurementUnits.FirstOrDefault(x => x.Id == SelectedMaterialItem.MeasurementUnitId);

        public MaterialMovementViewModel()
        {
            LoadAllDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadAllDataAsync()
        {
            try
            {
                await LoadMaterialItemsAsync();
                await LoadWarehousesAsync();
                await LoadSuppliersAsync();
                await LoadCategoriesAsync();
                await LoadMeasurementUnitsAsync();
                await LoadMaterialMovementsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних:\n{ex.Message}");
            }
        }

        private async Task LoadMaterialItemsAsync()
        {
            var items = await _materialItemApiService.GetAllAsync();
            MaterialItems = new ObservableCollection<MaterialItem>(items);
        }

        private async Task LoadWarehousesAsync()
        {
            var list = await _warehouseApiService.GetAllAsync();
            Warehouses = new ObservableCollection<Warehouse>(list);
        }

        private async Task LoadSuppliersAsync()
        {
            var list = await _supplierApiService.GetSuppliersAsync();
            Suppliers = new ObservableCollection<Supplier>(list);
        }

        private async Task LoadCategoriesAsync()
        {
            var list = await _categoryApiService.GetAllAsync();
            Categories = new ObservableCollection<Category>(list);
        }

        private async Task LoadMeasurementUnitsAsync()
        {
            var list = await _measurementUnitApiService.GetAllAsync();
            MeasurementUnits = new ObservableCollection<MeasurementUnit>(list);
        }

        [RelayCommand]
        private async Task LoadMaterialMovementsAsync()
        {
            var list = await _apiService.GetAllAsync();
            MaterialMovements = new ObservableCollection<MaterialMovement>(list);
        }

        [RelayCommand]
        private async Task AddMaterialMovement()
        {
            var form = new MaterialMovementForm(null, MaterialItems, Warehouses);
            if (form.ShowDialog() == true)
            {
                try
                {
                    var newMovement = form.Result;

                    await _apiService.AddAsync(newMovement);

                    await LoadMaterialMovementsAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при додаванні: {ex.Message}");
                }
            }
        }



        [RelayCommand]
        private async Task EditMaterialMovement(MaterialMovement? movement)
        {
            if (movement == null) return;

            var form = new MaterialMovementForm(movement, MaterialItems, Warehouses);
            if (form.ShowDialog() == true)
            {
                try
                {
                    var updatedMovement = form.Result;
                    await _apiService.UpdateAsync(updatedMovement);

                    var index = MaterialMovements.IndexOf(movement);
                    if (index >= 0)
                    {
                        MaterialMovements[index] = updatedMovement;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при редагуванні руху:\n{ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task DeleteMaterialMovement(MaterialMovement? movement)
        {
            if (movement == null) return;

            var confirm = MessageBox.Show("Ви впевнені, що хочете видалити рух матеріалу?", "Підтвердження", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    await _apiService.DeleteAsync(movement.Id);
                    MaterialMovements.Remove(movement);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при видаленні руху:\n{ex.Message}");
                }
            }
        }

        private readonly IPrinterService _printerService = new ZebraPrinterService("127.0.0.1");

        [RelayCommand]
        private async Task PrintLabel(MaterialMovement? movement)
        {
            if (movement == null) return;

            var form = new PrintLabelForm();
            if (form.ShowDialog() == true)
            {
                var settings = form.Result;

                // Створюємо копію руху з потрібними даними
                var enrichedMovement = new MaterialMovement
                {
                    Id = movement.Id,
                    BarcodeNumber = movement.BarcodeNumber,
                    MaterialItemName = SelectedMaterialItem?.Name ?? "-",
                    SupplierName = SelectedSupplier?.Name ?? "-",
                    MovementDate = movement.MovementDate,
                    Quantity = movement.Quantity,
                    WarehouseName = SelectedWarehouse?.Name ?? "_",
                    CategoryName = SelectedCategory?.Name ?? "_",
                };

                await _printerService.PrintMaterialMovementAsync(enrichedMovement, settings);
            }
        }

        partial void OnSelectedMaterialMovementChanged(MaterialMovement? value)
        {
            OnPropertyChanged(nameof(SelectedMaterialItem));
            OnPropertyChanged(nameof(SelectedSupplier));
            OnPropertyChanged(nameof(SelectedWarehouse));
            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(SelectedMeasurementUnit));
        }
    }
}
