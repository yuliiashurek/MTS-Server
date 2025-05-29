using Client.Models;
using Client.Services;
using Server.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace Client.Views
{
    public partial class MaterialItemForm : HandyControl.Controls.Window
    {

        public ObservableCollection<string> AvailableEmails { get; set; } = new();
        public ObservableCollection<string> SelectedEmails { get; set; } = new();
        private readonly UserApiService _userService = new(App.SharedHttpClient);

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


            var users = await _userService.GetAllAsync();
            AvailableEmails = new ObservableCollection<string>(users?
                .Where(u => !string.IsNullOrWhiteSpace(u.Email))
                .Select(u => u.Email) ?? Enumerable.Empty<string>());

            DataContext = null;
            DataContext = this;

            if (existing != null)
            {
                if (!string.IsNullOrWhiteSpace(existing.NotificationEmails))
                {
                    var selectedEmails = existing.NotificationEmails
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        foreach (var email in selectedEmails)
                        {
                            if (AvailableEmails.Contains(email))
                                EmailListBox.SelectedItems.Add(email);
                        }
                    }, DispatcherPriority.Loaded);
                }
            }

        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMeasurementUnit != null)
                MaterialItem.MeasurementUnitId = SelectedMeasurementUnit.Id;

            if (SelectedCategory != null)
                MaterialItem.CategoryId = SelectedCategory.Id;

            if (SelectedSupplier != null)
                MaterialItem.SupplierId = SelectedSupplier.Id;
            var selected = EmailListBox.SelectedItems.Cast<string>().ToList();
            MaterialItem.NotificationEmails = string.Join(",", selected);


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
