using Client.ViewModels;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Client
{
    public partial class MainViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = Properties.Resources.MainViewSuppliersMenu;

        [ObservableProperty]
        private UserControl currentContent;

        [ObservableProperty]
        private string selectedLanguage;

        public ObservableCollection<string> Languages { get; } = new() { "uk", "en" };
        public bool IsAdmin => SessionManager.Current?.Role == "Admin";


        public MainViewViewModel()
        {
            SelectedLanguage = LanguageSettings.Language;
            LoadLastSectionOrDefault();
        }

        private void LoadLastSectionOrDefault()
        {
            var section = Properties.Settings.Default.LastSection;
            Properties.Settings.Default.LastSection = "";
            Properties.Settings.Default.Save();

            if (section == nameof(WarehousesCommand))
                WarehousesCommand.Execute(null);
            else
                SuppliersCommand.Execute(null);
        }

        partial void OnSelectedLanguageChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && value != LanguageSettings.Language)
            {
                Properties.Settings.Default.LastSection = GetCurrentSection();
                Properties.Settings.Default.Save();
                LanguageSettings.ChangeCulture(value);
            }
        }

        private string GetCurrentSection()
        {
            if (CurrentContent is SuppliersControl)
                return nameof(SuppliersCommand);
            return "";
        }

        [RelayCommand]
        private void Suppliers()
        {
            Title = Properties.Resources.MainViewSuppliersMenu;
            CurrentContent = new SuppliersControl();
        }

        [RelayCommand]
        private void MyAccount()
        {
            Title = "Мій аккаунт";
            CurrentContent = new MyAccountControl();
        }

        [RelayCommand]
        private void Users()
        {
            Title = "Користувачі";
            CurrentContent = new UsersControl();
        }

        [RelayCommand]
        private void MeasurementUnits()
        {
            Title = "Одиниці вимірювання";
            CurrentContent = new MeasurementUnitsControl();
        }

        [RelayCommand]
        private void Categories()
        {
            Title = "Категорії";
            CurrentContent = new NamedEntitiesControl { DataContext = new CategoriesViewModel() };
        }

        [RelayCommand]
        private void Warehouses()
        {
            Title = "Складські приміщення";
            CurrentContent = new NamedEntitiesControl { DataContext = new WarehousesViewModel() };
        }

        [RelayCommand]
        private void MaterialItems()
        {
            Title = "Матеріали";
            CurrentContent = new MaterialItemsControl();
        }

        [RelayCommand]
        private void MaterialMovements()
        {
            Title = "Рух матеріалів";
            CurrentContent = new MaterialMovementsControl();
        }

    }
}
