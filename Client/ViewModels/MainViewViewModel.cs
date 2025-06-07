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

        private string _currentSection = string.Empty;


        public MainViewViewModel()
        {
            SelectedLanguage = LanguageSettings.Language;
            LoadLastSectionOrDefault();
        }

        private void LoadLastSectionOrDefault()
        {
            var section = Properties.Settings.Default.LastSection;

            var commandMap = new Dictionary<string, IRelayCommand>
            {
                { nameof(SuppliersControl), SuppliersCommand },
                { nameof(MyAccountControl), MyAccountCommand },
                { nameof(UsersControl), UsersCommand },
                { nameof(MeasurementUnitsControl), MeasurementUnitsCommand },
                { "categories", CategoriesCommand },
                { "warehouses", WarehousesCommand },
                { nameof(MaterialItemsControl), MaterialItemsCommand },
                { nameof(MaterialMovementsControl), MaterialMovementsCommand },
                { nameof(AllDashboardsControl), DashboardsCommand },
                { nameof(AllReportsControl), ReportingCommand },
                { nameof(OrganizationInfoControl), OrganizationCommand }
            };

            if (!string.IsNullOrEmpty(section) && commandMap.TryGetValue(section, out var command))
                command.Execute(null);
            else
                DashboardsCommand.Execute(null);
        }


        partial void OnSelectedLanguageChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && value != LanguageSettings.Language)
            {
                Properties.Settings.Default.LastSection = _currentSection;
                Properties.Settings.Default.Save();
                LanguageSettings.ChangeCulture(value);
            }
        }

        [RelayCommand]
        private void Suppliers()
        {
            Title = Properties.Resources.MainViewSuppliersMenu;
            CurrentContent = new SuppliersControl();
            _currentSection = nameof(SuppliersControl);
        }

        [RelayCommand]
        private void MyAccount()
        {
            Title = Properties.Resources.MainViewMyAccountMenu;
            CurrentContent = new MyAccountControl();
            _currentSection = nameof(MyAccountControl);
        }

        [RelayCommand]
        private void Users()
        {
            Title = Properties.Resources.MainViewUsersMenu;
            CurrentContent = new UsersControl();
            _currentSection = nameof(UsersControl);
        }

        [RelayCommand]
        private void MeasurementUnits()
        {
            Title = Properties.Resources.MainViewMeasurementUnitsMenu;
            CurrentContent = new MeasurementUnitsControl();
            _currentSection = nameof(MeasurementUnitsControl);
        }

        [RelayCommand]
        private void Categories()
        {
            Title = Properties.Resources.MainViewCategoriesMenu;
            CurrentContent = new NamedEntitiesControl { DataContext = new CategoriesViewModel() };
            _currentSection = "categories";
        }

        [RelayCommand]
        private void Warehouses()
        {
            Title = Properties.Resources.MainViewWarehousesMenu;
            CurrentContent = new NamedEntitiesControl { DataContext = new WarehousesViewModel() };
            _currentSection = "warehouses";
        }

        [RelayCommand]
        private void MaterialItems()
        {
            Title = Properties.Resources.MainViewMaterialsMenu;
            CurrentContent = new MaterialItemsControl();
            _currentSection = nameof(MaterialItemsControl);
        }

        [RelayCommand]
        private void MaterialMovements()
        {
            Title = Properties.Resources.MainViewMaterialMovements;
            CurrentContent = new MaterialMovementsControl();
            _currentSection = nameof(MaterialMovementsControl);
        }

        [RelayCommand]
        private void Dashboards()
        {
            Title = Properties.Resources.MainViewDashboardsMenu;
            CurrentContent = new AllDashboardsControl();
            _currentSection = nameof(AllDashboardsControl);
        }

        [RelayCommand]
        private void Reporting()
        {
            Title = Properties.Resources.MainViewReportingMenu;
            CurrentContent = new AllReportsControl();
            _currentSection = nameof(AllReportsControl);
        }

        [RelayCommand]
        private void Organization()
        {
            Title = Properties.Resources.MainViewOrganizationMenu;
            CurrentContent = new OrganizationInfoControl();
            _currentSection = nameof(OrganizationInfoControl);
        }
    }
}
