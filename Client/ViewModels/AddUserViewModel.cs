using Client.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels {

    public partial class AddUserViewModel : ObservableObject
    {
        private readonly UserApiService _userService = new();

        [ObservableProperty] private string email;
        [ObservableProperty] private string selectedRole = "Employee";

        public List<string> Roles { get; } = new() { "Employee", "Admin" };

        public event Action? OnCancel;
        public event Action? OnSaved;

        [RelayCommand]
        private void Cancel()
        {
            OnCancel?.Invoke();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email обов'язковий");
                return;
            }

            var dto = new CreateUserDto
            {
                Email = Email,
                Role = SelectedRole
            };

            var success = await _userService.CreateAsync(dto);

            if (success)
            {
                MessageBox.Show("Користувача додано");
                OnSaved?.Invoke();
            }
            else
            {
                MessageBox.Show("Не вдалося додати користувача");
            }
        }
    }
}
