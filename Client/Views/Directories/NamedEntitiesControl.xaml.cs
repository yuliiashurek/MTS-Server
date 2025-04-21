using System.Windows.Controls;
using System.Windows.Data;

namespace Client
{
    public partial class NamedEntitiesControl : UserControl
    {
        public NamedEntitiesControl()
        {
            InitializeComponent();
        }

        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.Row.Item is INamedEntity entity)
                entity.PreviousName = entity.Name;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DataContext is IBaseNamedEntityViewModel vm && e.Row.Item is INamedEntity entity)
            {
                var textBox = e.EditingElement as TextBox;
                var newName = textBox?.Text?.Trim();

                if (!vm.ValidateName(entity, newName, out var error))
                {
                    HandyControl.Controls.Growl.Error(error);
                    entity.Name = entity.PreviousName ?? entity.Name;
                    var binding = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
                    binding?.UpdateTarget();
                    return;
                }

                entity.Name = newName;
                vm.SaveCommand.Execute(entity);
            }
        }
    }
}
