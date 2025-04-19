using System.Windows;

namespace Client.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowLoginView()
        {
            MainContentControl.Content = new LoginView();
        }

        public void ShowMainView()
        {
            MainContentControl.Content = new MainView();
        }
    }
}
