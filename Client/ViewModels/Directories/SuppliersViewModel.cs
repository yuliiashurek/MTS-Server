using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client
{
    public partial class SuppliersViewModel : ObservableObject
    {
        private readonly SupplierApiService _apiService = new(App.SharedHttpClient);

        private List<Supplier> _allSuppliers = new();

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers = new();

        [ObservableProperty] private string nameFilter;
        [ObservableProperty] private string contactFilter;
        [ObservableProperty] private string phoneFilter;
        [ObservableProperty] private string emailFilter;
        [ObservableProperty] private string addressFilter;

        public SuppliersViewModel()
        {
            LoadSuppliersCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadSuppliersAsync()
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

        [RelayCommand]
        private void ApplyFilters()
        {
            var filtered = _allSuppliers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(NameFilter))
                filtered = filtered.Where(s => s.Name?.Contains(NameFilter, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(ContactFilter))
                filtered = filtered.Where(s => s.ContactPerson?.Contains(ContactFilter, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(PhoneFilter))
                filtered = filtered.Where(s => s.Phone?.Contains(PhoneFilter, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(EmailFilter))
                filtered = filtered.Where(s => s.Email?.Contains(EmailFilter, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(AddressFilter))
                filtered = filtered.Where(s => s.Address?.Contains(AddressFilter, StringComparison.OrdinalIgnoreCase) == true);

            Suppliers = new ObservableCollection<Supplier>(filtered);
        }

        [RelayCommand]
        private void ClearFilters()
        {
            NameFilter = ContactFilter = PhoneFilter = EmailFilter = AddressFilter = string.Empty;
            ApplyFilters();
        }

        [RelayCommand]
        private async Task AddSupplier()
        {
            var form = new SupplierForm();
            if (form.ShowDialog() == true)
            {
                var newSupplier = form.Supplier;
                await _apiService.AddSupplierAsync(newSupplier);
                _allSuppliers.Add(newSupplier);
                ApplyFilters();
            }
        }

        [RelayCommand]
        private async Task EditSupplier(Supplier selected)
        {
            if (selected == null) return;

            var form = new SupplierForm(selected);
            if (form.ShowDialog() == true)
            {
                var updated = form.Supplier;
                await _apiService.UpdateSupplierAsync(updated);
                var index = _allSuppliers.IndexOf(selected);
                if (index >= 0) _allSuppliers[index] = updated;
                ApplyFilters();
            }
        }

        [RelayCommand]
        private async Task DeleteSupplier(Supplier selected)
        {
            if (selected == null) return;

            var message = string.Format(Properties.Resources.SuppliersControlDeleteSupplierConfirmation, selected.Name);
            var confirm = MessageBox.Show(message, Properties.Resources.DeleteConfirmation, MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                await _apiService.DeleteSupplierAsync(selected.Id);
                _allSuppliers.Remove(selected);
                ApplyFilters();
            }
        }
    }
}
