using Client.Services;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Client.ViewModels
{

    public partial class UsersViewModel : ObservableObject
    {
        private readonly UserApiService _userService = new();

        [ObservableProperty]
        private ObservableCollection<UserDto> users = [];

        [ObservableProperty]
        private UserControl? addUserForm;

        public UsersViewModel()
        {
            LoadAsync();
        }

        private async Task LoadAsync()
        {
            var result = await _userService.GetAllAsync();
            if (result != null)
                Users = new ObservableCollection<UserDto>(result);
        }

        [RelayCommand]
        public async Task DeleteAsync(Guid userId)
        {
            var confirm = MessageBox.Show("Ви дійсно хочете видалити користувача?", "Підтвердження",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            var success = await _userService.DeleteAsync(userId);
            if (success)
            {
                var user = Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                    Users.Remove(user);
            }
            else
            {
                MessageBox.Show("Не вдалося видалити користувача");
            }
        }

        [RelayCommand]
        public async Task InviteAsync(Guid userId)
        {
            var success = await _userService.InviteAsync(userId);
            if (success)
            {
                MessageBox.Show("Запрошення надіслано", "Успіх");
            }
            else
            {
                MessageBox.Show("Не вдалося надіслати запрошення", "Помилка");
            }
        }

        [RelayCommand]
        private void ShowAddUserForm()
        {
            var form = new AddUserForm();
            if (form.DataContext is AddUserViewModel vm)
            {
                vm.OnCancel += () => AddUserForm = null;
                vm.OnSaved += async () =>
                {
                    AddUserForm = null;
                    await LoadAsync();
                };
            }

            AddUserForm = form;
        }
    }
}