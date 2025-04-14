using System.Windows.Controls;

namespace Client.Views
{
    public partial class SuppliersControl : UserControl
    {
        public SuppliersControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.SuppliersViewModel();
        }
    }
}
