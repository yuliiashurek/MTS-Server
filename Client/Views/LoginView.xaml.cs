using Client.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
   public partial class LoginView : UserControl
   {
        public LoginView()
        {
            InitializeComponent();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        private void AdminPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.AdminPassword = ((PasswordBox)sender).Password;
        }

    }

}
