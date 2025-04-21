using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using HandyControl.Controls;

namespace Client
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string? errorMessage;

        [ObservableProperty] private string organizationName;
        [ObservableProperty] private string adminEmail;
        [ObservableProperty] private string adminPassword;

        [RelayCommand]
        public async Task LoginAsync()
        {
            await loginAsync(Email, Password);
        }

        [RelayCommand]
        public async Task RegisterAsync()
        {
            ErrorMessage = null;
            var result = await App.AuthService.RegisterOrganizationAsync(new RegisterOrganizationDto
            {
                OrganizationName = OrganizationName,
                AdminEmail = AdminEmail,
                AdminPassword = AdminPassword
            });

            if (!result)
            {
                ErrorMessage = "Не вдалося зареєструвати організацію.";
            }
            else
            {
                Growl.Success("Організацію створено");
            }
            await loginAsync(AdminEmail, AdminPassword);
        }

        private async Task loginAsync(string email, string password)
        {
            ErrorMessage = null;
            var session = await App.AuthService.LoginAsync(email, password);
            if (session == null)
            {
                ErrorMessage = "Невірний email або пароль";
                return;
            }
            SessionManager.SetSession(session);
            App.AuthService.AddAuthHeader();
            if (App.Current.MainWindow is MainWindow mw)
                mw.ShowMainView();
        }
    }
}
