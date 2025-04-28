using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client
{
    public abstract partial class BaseNamedEntityViewModel<T> : ObservableObject, IBaseNamedEntityViewModel
        where T : class, INamedEntity, new()
    {

        public BaseNamedEntityViewModel()
        {
            _ = LoadAsync();
        }

        public ObservableCollection<T> Items { get; } = new();

        ObservableCollection<INamedEntity> IBaseNamedEntityViewModel.Items => new(Items.Cast<INamedEntity>());
        ICommand IBaseNamedEntityViewModel.AddNewRowCommand => AddNewRowCommand;
        ICommand IBaseNamedEntityViewModel.SaveCommand => SaveCommand;
        ICommand IBaseNamedEntityViewModel.DeleteCommand => DeleteCommand;
        bool IBaseNamedEntityViewModel.ValidateName(INamedEntity entity, string? newName, out string? error)
            => ValidateName((T)entity, newName, out error);

        protected abstract Func<Task<List<T>>> LoadFromApi { get; }
        protected abstract Func<T, Task> AddToApi { get; }
        protected abstract Func<T, Task> UpdateToApi { get; }
        protected abstract Func<T, Task> DeleteFromApi { get; }

        [RelayCommand]
        public async Task LoadAsync()
        {
            Items.Clear();
            var result = await LoadFromApi();
            foreach (var item in result)
                Items.Add(item);
        }

        [RelayCommand]
        public void AddNewRow()
        {
            string baseName = "NewItem";
            int counter = 0;
            string newName;
            do
            {
                newName = counter == 0 ? baseName : $"{baseName}{counter}";
                counter++;
            }
            while (Items.Any(c => c.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)));

            var newItem = new T
            {
                Id = Guid.NewGuid(),
                Name = newName,
            };
            Items.Add(newItem);

            AddToApi(newItem);
        }

        [RelayCommand]
        public async Task SaveAsync(T item)
        {
            if (!ValidateName(item, item.Name, out var error))
            {
                Growl.Error(error);
                return;
            }
            await UpdateToApi(item);
        }

        [RelayCommand]
        public async Task DeleteAsync(T item)
        {
            await DeleteFromApi(item);
            Items.Remove(item);
        }

        public bool ValidateName(T item, string? newName, out string? error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(newName))
            {
                error = "Назва не може бути порожньою";
                return false;
            }

            if (Items.Any(c => !Equals(c.Id, item.Id) && c.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                error = "Назва має бути унікальною";
                return false;
            }

            return true;
        }
    }
}
