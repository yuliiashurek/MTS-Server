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
            Title = Properties.Resources.MainViewMyAccountMenu;
            CurrentContent = new MyAccountControl();
        }

        [RelayCommand]
        private void Users()
        {
            Title = Properties.Resources.MainViewUsersMenu;
            CurrentContent = new UsersControl();
        }

        [RelayCommand]
        private void MeasurementUnits()
        {
            Title = Properties.Resources.MainViewMeasurementUnitsMenu;
            CurrentContent = new MeasurementUnitsControl();
        }

        [RelayCommand]
        private void Categories()
        {
            Title = Properties.Resources.MainViewCategoriesMenu;
            CurrentContent = new NamedEntitiesControl { DataContext = new CategoriesViewModel() };
        }

        [RelayCommand]
        private void Warehouses()
        {
            Title = Properties.Resources.MainViewWarehousesMenu;
            CurrentContent = new NamedEntitiesControl { DataContext = new WarehousesViewModel() };
        }

        [RelayCommand]
        private void MaterialItems()
        {
            Title = Properties.Resources.MainViewMaterialsMenu;
            CurrentContent = new MaterialItemsControl();
        }

        [RelayCommand]
        private void MaterialMovements()
        {
            Title = Properties.Resources.MainViewMaterialMovements;
            CurrentContent = new MaterialMovementsControl();
        }

        [RelayCommand]
        private void Dashboards()
        {
            Title = Properties.Resources.MainViewDashboardsMenu;
            CurrentContent = new AllDashboardsControl();
        }

        [RelayCommand]
        private void Reporting()
        {
            Title = Properties.Resources.MainViewReportingMenu;
            CurrentContent = new AllReportsControl();
        }

        [RelayCommand]
        private void Organization()
        {
            Title = Properties.Resources.MainViewOrganizationMenu;
            CurrentContent = new OrganizationInfoControl();
        }
    }
}
