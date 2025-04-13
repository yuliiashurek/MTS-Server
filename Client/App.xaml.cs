using Client.Localization;
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

        public App()
        {
            this.DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show($"ПОМИЛКАААААААА: {e.Exception.Message}", "ПОМИЛКАААААААА", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
        }
    }
}