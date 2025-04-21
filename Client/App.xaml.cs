using AutoMapper;
using System.Net.Http;
using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        public static IMapper Mapper { get; private set; }
        public static HttpClient SharedHttpClient { get; private set; }

        public static AuthApiService AuthService { get; private set; }

        public App()
        {
            LanguageSettings.ApplySavedCulture();
            SessionManager.LoadSession();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            Mapper = config.CreateMapper();

            var handler = new AuthHttpClientHandler(new HttpClientHandler());
            SharedHttpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7262/api/")
            };
            AuthService = new AuthApiService(SharedHttpClient);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            MainWindow.Show();

            var session = await SessionManager.TryRestoreSessionAsync();
            if (session != null)
            {
                mainWindow.ShowMainView();
            }
            else
            {
                mainWindow.ShowLoginView();
            }
        }
    }
}
