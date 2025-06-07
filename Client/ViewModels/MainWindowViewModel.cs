using CommunityToolkit.Mvvm.ComponentModel;

namespace Client
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public event Action? RequestMaximizeWindow;

        [ObservableProperty]
        private object currentView;

        public void ShowLoginView()
        {
            CurrentView = new LoginView();
        }

        public void ShowMainView()
        {
            RequestMaximizeWindow?.Invoke();
            CurrentView = new MainView();
        }
    }

}
