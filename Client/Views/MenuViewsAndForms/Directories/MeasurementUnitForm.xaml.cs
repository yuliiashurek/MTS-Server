using HandyControl.Controls;
using System.Windows;

namespace Client
{
    public partial class MeasurementUnitForm : HandyControl.Controls.Window
    {
        public MeasurementUnit Unit { get; private set; }

        public MeasurementUnitForm(MeasurementUnit? unit = null)
        {
            InitializeComponent();

            Unit = unit != null ? new MeasurementUnit
            {
                Id = unit.Id,
                FullName = unit.FullName,
                ShortName = unit.ShortName
            } : new MeasurementUnit();

            FullNameBox.Text = Unit.FullName;
            ShortNameBox.Text = Unit.ShortName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameBox.Text))
            {
                Growl.Error("Повна назва обов’язкова.");
                return;
            }

            Unit.FullName = FullNameBox.Text;
            Unit.ShortName = ShortNameBox.Text;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
