using System.Windows.Controls;

namespace Client
{
    public partial class SuppliersControl : UserControl
    {
        public SuppliersControl()
        {
            InitializeComponent();
            DataContext = new SuppliersViewModel();
        }
    }
}
