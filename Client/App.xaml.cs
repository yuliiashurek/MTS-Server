using AutoMapper;
using Client.Localization;
using Client.Mappings;
using Client.Views;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LanguageSettings.ApplySavedCulture(); 
            var mainView = new MainView();       
            MainWindow = mainView;               
            mainView.Show();                     
        }

        public static IMapper Mapper { get; private set; }
        public App()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = config.CreateMapper();

            this.DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show($"ПОМИЛКАААААА {e.Exception.Message}", "ПОМИЛКАААААА", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
        }
    }
}