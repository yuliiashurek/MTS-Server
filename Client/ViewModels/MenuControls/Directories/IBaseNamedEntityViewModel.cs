using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client
{
    public interface IBaseNamedEntityViewModel
    {
        ObservableCollection<INamedEntity> Items { get; }

        ICommand AddNewRowCommand { get; }
        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }

        bool ValidateName(INamedEntity entity, string? newName, out string? error);
    }
}
