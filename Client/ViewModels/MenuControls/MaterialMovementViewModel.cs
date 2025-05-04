using Client.Models;
using Client.Services;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using Server.Shared.Enums;
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
        private readonly IPrinterService _printerService = new ZebraPrinterService("127.0.0.1");

        [ObservableProperty] private ObservableCollection<MaterialMovement> materialMovements = new();
        [ObservableProperty] private ObservableCollection<MaterialItem> materialItems = new();
        [ObservableProperty] private ObservableCollection<Warehouse> warehouses = new();
        [ObservableProperty] private ObservableCollection<Supplier> suppliers = new();
        [ObservableProperty] private ObservableCollection<Category> categories = new();
        [ObservableProperty] private ObservableCollection<MeasurementUnit> measurementUnits = new();

        [ObservableProperty] private Category? selectedCategoryFilter;
        [ObservableProperty] private Warehouse? selectedWarehouseFilter;
        [ObservableProperty] private Supplier? selectedSupplierFilter;
        [ObservableProperty] private MaterialMovement? selectedMaterialMovement;

        public ObservableCollection<MaterialMovement> FilteredMaterialMovements =>
            new(MaterialMovements.Where(m =>
                (SelectedCategoryFilter == null || SelectedCategoryFilter.Id == Guid.Empty || MaterialItems.FirstOrDefault(i => i.Id == m.MaterialItemId)?.CategoryId == SelectedCategoryFilter.Id) &&
                (SelectedWarehouseFilter == null || SelectedWarehouseFilter.Id == Guid.Empty || m.WarehouseId == SelectedWarehouseFilter.Id) &&
                (SelectedSupplierFilter == null || SelectedSupplierFilter.Id == Guid.Empty || MaterialItems.FirstOrDefault(i => i.Id == m.MaterialItemId)?.SupplierId == SelectedSupplierFilter.Id)
            ));

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
            MaterialItems = new(items);
        }

        private async Task LoadWarehousesAsync()
        {
            var list = await _warehouseApiService.GetAllAsync();
            list.Insert(0, new Warehouse { Id = Guid.Empty, Name = "Усі" });
            Warehouses = new(list);
        }

        private async Task LoadSuppliersAsync()
        {
            var list = await _supplierApiService.GetSuppliersAsync();
            list.Insert(0, new Supplier { Id = Guid.Empty, Name = "Усі" });
            Suppliers = new(list);
        }

        private async Task LoadCategoriesAsync()
        {
            var list = await _categoryApiService.GetAllAsync();
            list.Insert(0, new Category { Id = Guid.Empty, Name = "Усі" });
            Categories = new(list);
        }

        private async Task LoadMeasurementUnitsAsync()
        {
            var list = await _measurementUnitApiService.GetAllAsync();
            MeasurementUnits = new(list);
        }

        [RelayCommand]
        private async Task LoadMaterialMovementsAsync()
        {
            var list = await _apiService.GetAllAsync();
            MaterialMovements = new(list);
            OnPropertyChanged(nameof(FilteredMaterialMovements));
            SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
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

        public async void AddMaterialMovementWriteOff(MaterialMovementDto newMovement)
        {
            var res = App.Mapper.Map<MaterialMovement>(newMovement);
            MaterialMovements.Add(res);
            await _apiService.AddAsync(res);
            await LoadMaterialMovementsAsync();
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
                    updatedMovement.Id = movement.Id;
                    updatedMovement.OrganizationId = movement.OrganizationId;
                    updatedMovement.BarcodeNumber = movement.BarcodeNumber;
                    await _apiService.UpdateAsync(updatedMovement);

                    var index = MaterialMovements.IndexOf(movement);
                    if (index >= 0)
                    {
                        MaterialMovements[index] = updatedMovement;
                        OnPropertyChanged(nameof(FilteredMaterialMovements));
                        SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
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
                    OnPropertyChanged(nameof(FilteredMaterialMovements));
                    SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при видаленні руху:\n{ex.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task PrintLabel(MaterialMovement? movement)
        {
            if (movement == null) return;

            var form = new PrintLabelForm();
            if (form.ShowDialog() == true)
            {
                var settings = form.Result;

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

        [RelayCommand]
        private void WriteOffByBarcode()
        {
            var inputDialog = new InputBarcodeWindow();
            if (inputDialog.ShowDialog() != true || string.IsNullOrWhiteSpace(inputDialog.Barcode))
                return;

            var barcode = inputDialog.Barcode.Trim();

            var movement = MaterialMovements
                .FirstOrDefault(m => m.BarcodeNumber == barcode && m.MovementType == 0);

            if (movement is null)
            {
                MessageBox.Show($"Поставку з Barcode '{barcode}' не знайдено!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // мапінг через AutoMapper
            var sourceMovement = App.Mapper.Map<MaterialMovementDto>(movement);

            var writeOffVm = new WriteOffFormViewModel(sourceMovement, this);
            var writeOffForm = new WriteOffForm { DataContext = writeOffVm };
            writeOffForm.ShowDialog();
        }



        partial void OnSelectedMaterialMovementChanged(MaterialMovement? value)
        {
            OnPropertyChanged(nameof(SelectedMaterialItem));
            OnPropertyChanged(nameof(SelectedSupplier));
            OnPropertyChanged(nameof(SelectedWarehouse));
            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(SelectedMeasurementUnit));
        }

        partial void OnSelectedCategoryFilterChanged(Category? value)
        {
            OnPropertyChanged(nameof(FilteredMaterialMovements));
            SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
        }

        partial void OnSelectedWarehouseFilterChanged(Warehouse? value)
        {
            OnPropertyChanged(nameof(FilteredMaterialMovements));
            SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
        }

        partial void OnSelectedSupplierFilterChanged(Supplier? value)
        {
            OnPropertyChanged(nameof(FilteredMaterialMovements));
            SelectedMaterialMovement = FilteredMaterialMovements.FirstOrDefault();
        }
    }
}
