using AutoMapper;
using Client.Helpers;
using Client.Localization;
using Client.Mappings;
using Client.Views;
using Server.Shared.DTOs;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        public static IMapper Mapper { get; private set; }
        public static HttpClient HttpClient { get; private set; }
        public static AuthApiService AuthApiService { get; private set; }

        public App()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            Mapper = config.CreateMapper();

            HttpClient = new HttpClient(new AuthHttpClientHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri("https://localhost:7262/") // заміни на свою адресу сервера
            };

            AuthApiService = new AuthApiService(HttpClient);

            this.DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show($"ПОМИЛКА: {e.Exception.Message}", "ПОМИЛКА", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LanguageSettings.ApplySavedCulture();

            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            MainWindow.Show();

            var session = await TryRestoreSessionAsync();
            if (session != null)
            {
                SessionManager.SetSession(session);
                App.AuthApiService.AddAuthHeader();
                mainWindow.ShowMainView();
            }
            else
            {
                mainWindow.ShowLoginView();
            }
        }


        private async Task<UserSession?> TryRestoreSessionAsync()
        {
            SessionManager.LoadSession();
            var session = SessionManager.Current;
            if (session == null) return null;

            var newAccessToken = await AuthApiService.RefreshTokenAsync(session.RefreshToken);
            if (string.IsNullOrEmpty(newAccessToken))
            {
                SessionManager.ClearSession();
                try { File.Delete("user.config"); } catch { }
                return null;
            }

            session.AccessToken = newAccessToken;
            SessionManager.SetSession(session);
            AuthApiService.AddAuthHeader();
            return session;
        }
    }
}
