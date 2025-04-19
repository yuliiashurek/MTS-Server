using Client.Views;
using Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Server.Shared.DTOs;
using System.Windows;
using System.Diagnostics.Eventing.Reader;

namespace Client.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string? errorMessage;

        // Для реєстрації
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
            var result = await App.AuthApiService.RegisterOrganizationAsync(new RegisterOrganizationDto
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
                MessageBox.Show("Організацію створено. Тепер ви можете увійти.", "Успіх");
            }
            await loginAsync(AdminEmail, AdminPassword);
        }

        private async Task loginAsync(string email, string password)
        {
            ErrorMessage = null;
            var session = await App.AuthApiService.LoginAsync(email, password);
            if (session == null)
            {
                ErrorMessage = "Невірний email або пароль";
                return;
            }

            SessionManager.SetSession(session);
            App.AuthApiService.AddAuthHeader();
            if (App.Current.MainWindow is MainWindow mw)
                mw.ShowMainView();
        }
    }
}
