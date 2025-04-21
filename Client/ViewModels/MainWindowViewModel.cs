using CommunityToolkit.Mvvm.ComponentModel;

namespace Client
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private object currentView;

        public void ShowLoginView()
        {
            CurrentView = new LoginView();
        }

        public void ShowMainView()
        {
            CurrentView = new MainView();
        }
    }
}
