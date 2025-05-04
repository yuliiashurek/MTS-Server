using Client.Models;
using Client.Services;
using Server.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client.Views
{
    public partial class MaterialItemForm : Window
    {
        public MaterialItem MaterialItem { get; private set; }

        public ObservableCollection<MeasurementUnit> MeasurementUnits { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Supplier> Suppliers { get; set; } = new();

        public MeasurementUnit? SelectedMeasurementUnit { get; set; }
        public Category? SelectedCategory { get; set; }
        public Supplier? SelectedSupplier { get; set; }

        private readonly MeasurementUnitApiService _unitService = new(App.SharedHttpClient);
        private readonly CategoryApiService _categoryService = new(App.SharedHttpClient);
        private readonly SupplierApiService _supplierService = new(App.SharedHttpClient);

        public MaterialItemForm()
        {
            InitializeComponent();
            MaterialItem = new MaterialItem();
            DataContext = this;
            LoadDictionaries();
        }

        public MaterialItemForm(MaterialItem existing)
        {
            InitializeComponent();
            MaterialItem = new MaterialItem
            {
                Id = existing.Id,
                OrganizationId = existing.OrganizationId,
                Name = existing.Name,
                MinimumStock = existing.MinimumStock,
                MeasurementUnitId = existing.MeasurementUnitId,
                CategoryId = existing.CategoryId,
                SupplierId = existing.SupplierId
            };

            DataContext = this;

            LoadDictionaries(existing);
        }

        private async void LoadDictionaries(MaterialItem? existing = null)
        {
            var units = await _unitService.GetAllAsync();
            MeasurementUnits = new ObservableCollection<MeasurementUnit>(units);

            var categories = await _categoryService.GetAllAsync();
            Categories = new ObservableCollection<Category>(categories);

            var suppliers = await _supplierService.GetSuppliersAsync();
            Suppliers = new ObservableCollection<Supplier>(suppliers);

            if (existing != null)
            {
                SelectedMeasurementUnit = MeasurementUnits.FirstOrDefault(x => x.Id == existing.MeasurementUnitId);
                SelectedCategory = Categories.FirstOrDefault(x => x.Id == existing.CategoryId);
                SelectedSupplier = Suppliers.FirstOrDefault(x => x.Id == existing.SupplierId);
            }

            DataContext = null;
            DataContext = this;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMeasurementUnit != null)
                MaterialItem.MeasurementUnitId = SelectedMeasurementUnit.Id;

            if (SelectedCategory != null)
                MaterialItem.CategoryId = SelectedCategory.Id;

            if (SelectedSupplier != null)
                MaterialItem.SupplierId = SelectedSupplier.Id;

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
