using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using HandyControl.Controls;


namespace Client {

    public partial class AddUserViewModel : ObservableObject
    {
        private readonly UserApiService _userService = new(App.SharedHttpClient);

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
                Growl.Warning("Email обов'язковий");
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
                Growl.Success("Користувача додано");
                OnSaved?.Invoke();
            }
            else
            {
                Growl.Error("Не вдалося додати користувача");
            }
        }
    }
}
