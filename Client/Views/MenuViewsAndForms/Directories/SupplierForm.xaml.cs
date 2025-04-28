using HandyControl.Controls;
using System.Windows;

namespace Client
{
    public partial class SupplierForm : System.Windows.Window
    {
        public Supplier Supplier { get; private set; }

        public SupplierForm(Supplier? supplier = null)
        {
            InitializeComponent();

            Supplier = supplier != null ? new Supplier
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address
            } : new Supplier();

            NameBox.Text = Supplier.Name;
            ContactBox.Text = Supplier.ContactPerson;
            PhoneBox.Text = Supplier.Phone;
            EmailBox.Text = Supplier.Email;
            AddressBox.Text = Supplier.Address;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                Growl.Error("Ім’я постачальника є обов’язковим.");
                return;
            }

            Supplier.Name = NameBox.Text;
            Supplier.ContactPerson = ContactBox.Text;
            Supplier.Phone = PhoneBox.Text;
            Supplier.Email = EmailBox.Text;
            Supplier.Address = AddressBox.Text;

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
