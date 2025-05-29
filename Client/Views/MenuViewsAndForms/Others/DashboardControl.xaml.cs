
using Client.ViewModels.MenuControls;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is DashboardViewModel vm)
            {
                await vm.LoadDataAsync();
            }
        }
    }
}
