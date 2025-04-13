using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class SuppliersControl : UserControl
    {
        private readonly SupplierApiService _apiService = new();
        private List<Supplier> _allSuppliers = new();
        public ObservableCollection<Supplier> Suppliers { get; set; } = new();

        public SuppliersControl()
        {
            InitializeComponent();
            SuppliersGrid.ItemsSource = Suppliers;
            LoadSuppliersAsync();
        }

        private async void LoadSuppliersAsync()
        {
            try
            {
                _allSuppliers = (await _apiService.GetSuppliersAsync()).ToList();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.SuppliersControlLoadSuppliersError + "\n" + ex.Message);
            }
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            NameFilterBox.Text = "";
            ContactFilterBox.Text = "";
            PhoneFilterBox.Text = "";
            EmailFilterBox.Text = "";
            AddressFilterBox.Text = "";
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allSuppliers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(NameFilterBox.Text))
                filtered = filtered.Where(s => s.Name?.Contains(NameFilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(ContactFilterBox.Text))
                filtered = filtered.Where(s => s.ContactPerson?.Contains(ContactFilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(PhoneFilterBox.Text))
                filtered = filtered.Where(s => s.Phone?.Contains(PhoneFilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(EmailFilterBox.Text))
                filtered = filtered.Where(s => s.Email?.Contains(EmailFilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(AddressFilterBox.Text))
                filtered = filtered.Where(s => s.Address?.Contains(AddressFilterBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) == true);

            Suppliers.Clear();
            foreach (var supplier in filtered)
                Suppliers.Add(supplier);
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selected)
            {
                var form = new SupplierForm(selected)
                {
                    Owner = Window.GetWindow(this)
                };

                if (form.ShowDialog() == true)
                {
                    var updated = form.Supplier;
                    await _apiService.UpdateSupplierAsync(updated);

                    var index = _allSuppliers.IndexOf(selected);
                    if (index >= 0) _allSuppliers[index] = updated;

                    ApplyFilters();
                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SuppliersGrid.SelectedItem is Supplier selected)
            {
                var message = string.Format(Properties.Resources.SuppliersControlDeleteSupplierConfirmation, selected.Name);
                var confirm = MessageBox.Show(message, Properties.Resources.DeleteConfirmation, MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _apiService.DeleteSupplierAsync(selected.Id);
                        _allSuppliers.Remove(selected);
                        ApplyFilters();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Properties.Resources.SuppliersControlDeleteError + "\n" + ex.Message);
                    }
                }
            }
        }

        private async void AddNew_Click(object sender, RoutedEventArgs e)
        {
            var form = new SupplierForm
            {
                Owner = Window.GetWindow(this)
            };

            if (form.ShowDialog() == true)
            {
                var newSupplier = form.Supplier;
                await _apiService.AddSupplierAsync(newSupplier);
                _allSuppliers.Add(newSupplier);
                ApplyFilters();
            }
        }
    }
}
