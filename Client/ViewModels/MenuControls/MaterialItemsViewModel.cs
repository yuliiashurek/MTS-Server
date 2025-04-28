using Client.Models;
using Client.Services;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client.ViewModels
{
    public partial class MaterialItemsViewModel : ObservableObject
    {
        private readonly MaterialItemApiService _apiService = new(App.SharedHttpClient);
        private readonly SupplierApiService _supplierApiService = new(App.SharedHttpClient);
        private readonly CategoryApiService _categoryApiService = new(App.SharedHttpClient);
        private readonly MeasurementUnitApiService _unitApiService = new(App.SharedHttpClient);

        private List<MaterialItem> _allMaterials = new();

        [ObservableProperty]
        private ObservableCollection<MaterialItem> materialItems = new();

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers = new();

        [ObservableProperty]
        private ObservableCollection<Category> categories = new();

        [ObservableProperty]
        private ObservableCollection<MeasurementUnit> measurementUnits = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedMeasurementUnit))]
        [NotifyPropertyChangedFor(nameof(SelectedCategory))]
        [NotifyPropertyChangedFor(nameof(SelectedSupplier))]
        private MaterialItem? selectedMaterialItem;

        public MeasurementUnit? SelectedMeasurementUnit =>
            SelectedMaterialItem == null ? null : MeasurementUnits.FirstOrDefault(x => x.Id == SelectedMaterialItem.MeasurementUnitId);

        public Category? SelectedCategory =>
            SelectedMaterialItem == null ? null : Categories.FirstOrDefault(x => x.Id == SelectedMaterialItem.CategoryId);

        public Supplier? SelectedSupplier =>
            SelectedMaterialItem == null ? null : Suppliers.FirstOrDefault(x => x.Id == SelectedMaterialItem.SupplierId);

        public MaterialItemsViewModel()
        {
            LoadDictionaries();
            LoadMaterialItemsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadMaterialItemsAsync()
        {
            try
            {
                _allMaterials = (await _apiService.GetAllAsync()).ToList();
                MaterialItems = new ObservableCollection<MaterialItem>(_allMaterials);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження матеріалів\n" + ex.Message);
            }
        }

        [RelayCommand]
        private async Task AddMaterialItem()
        {
            var form = new MaterialItemForm();
            if (form.ShowDialog() == true)
            {
                var newItem = form.MaterialItem;
                await _apiService.AddAsync(newItem);
                _allMaterials.Add(newItem);
                MaterialItems.Add(newItem);
            }
        }

        [RelayCommand]
        private async Task EditMaterialItem(MaterialItem? selected)
        {
            if (selected == null) return;

            var form = new MaterialItemForm(selected);
            if (form.ShowDialog() == true)
            {
                var updated = form.MaterialItem;
                await _apiService.UpdateAsync(updated);
                var index = _allMaterials.IndexOf(selected);
                if (index >= 0) _allMaterials[index] = updated;
                MaterialItems = new ObservableCollection<MaterialItem>(_allMaterials);
            }
        }

        [RelayCommand]
        private async Task DeleteMaterialItem(MaterialItem? selected)
        {
            if (selected == null) return;

            var confirm = MessageBox.Show($"Ви впевнені, що хочете видалити матеріал '{selected.Name}'?", "Підтвердження видалення", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                await _apiService.DeleteAsync(selected.Id);
                _allMaterials.Remove(selected);
                MaterialItems.Remove(selected);
            }
        }

        private async void LoadDictionaries()
        {
            Suppliers = new ObservableCollection<Supplier>(await _supplierApiService.GetSuppliersAsync());
            Categories = new ObservableCollection<Category>(await _categoryApiService.GetAllAsync());
            MeasurementUnits = new ObservableCollection<MeasurementUnit>(await _unitApiService.GetAllAsync());
        }
    }
}
