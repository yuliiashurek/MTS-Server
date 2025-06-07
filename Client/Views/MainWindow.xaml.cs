using System.Windows;

namespace Client
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
            WindowState = WindowState.Maximized;
        }
    }
}
