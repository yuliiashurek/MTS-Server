using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace Client
{
    public partial class MeasurementUnitsViewModel : ObservableObject
    {
        private readonly MeasurementUnitApiService _apiService = new(App.SharedHttpClient);

        [ObservableProperty]
        private ObservableCollection<MeasurementUnit> units = new();

        public MeasurementUnitsViewModel()
        {
            LoadCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            var list = await _apiService.GetAllAsync();
            Units = new ObservableCollection<MeasurementUnit>(list);
        }

        [RelayCommand]
        private async Task AddUnit()
        {
            var form = new MeasurementUnitForm();
            if (form.ShowDialog() == true)
            {
                var unit = form.Unit;
                await _apiService.AddAsync(unit);
                Units.Add(unit);
            }
        }

        [RelayCommand]
        private async Task EditUnit(MeasurementUnit selected)
        {
            if (selected == null) return;

            var form = new MeasurementUnitForm(selected);
            if (form.ShowDialog() == true)
            {
                var updated = form.Unit;
                await _apiService.UpdateAsync(updated);
                var index = Units.IndexOf(selected);
                if (index >= 0) Units[index] = updated;
            }
        }

        [RelayCommand]
        private async Task DeleteUnit(MeasurementUnit selected)
        {
            if (selected == null) return;

            var confirm = System.Windows.MessageBox.Show($"Видалити '{selected.FullName}'?", "Підтвердження", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                await _apiService.DeleteAsync(selected.Id);
                Units.Remove(selected);
            }
        }
    }
}
