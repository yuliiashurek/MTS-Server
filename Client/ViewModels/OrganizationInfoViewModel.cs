using Client.Models;
using Client.Services.ApiServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public partial class OrganizationInfoViewModel : ObservableObject
    {
        private readonly OrganizationApiService _organizationApiService = new();

        private Organization? _original;

        [ObservableProperty]
        private Organization organization = new();

        [ObservableProperty]
        private bool isModified;

        public OrganizationInfoViewModel()
        {
            LoadOrganizationAsync();
        }

        private async void LoadOrganizationAsync()
        {
            var result = await _organizationApiService.GetMyOrganizationAsync();
            if (result is not null)
            {
                SetOrganization(result);
            }
        }

        private void SetOrganization(Organization newOrg)
        {
            Organization.PropertyChanged -= OnOrganizationPropertyChanged; 
            Organization = newOrg;
            Organization.PropertyChanged += OnOrganizationPropertyChanged;

            _original = DeepClone(newOrg);
            IsModified = false;
        }

        private void OnOrganizationPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckIfModified();
        }

        [RelayCommand(CanExecute = nameof(IsModified))]
        private async Task SaveAsync()
        {
            bool success = await _organizationApiService.UpdateMyOrganizationAsync(Organization);
            if (success)
            {
                Growl.Success("Зміни успішно збережено", "Успіх");
                SetOrganization(DeepClone(Organization));
            }
            else
            {
                Growl.Error("Помилка збереження", "Помилка");
            }
        }

        private void CheckIfModified()
        {
            if (_original == null)
            {
                IsModified = true;
                return;
            }

            var currentJson = JsonSerializer.Serialize(Organization);
            var originalJson = JsonSerializer.Serialize(_original);
            IsModified = currentJson != originalJson;
        }

        private Organization DeepClone(Organization org)
        {
            return JsonSerializer.Deserialize<Organization>(
                JsonSerializer.Serialize(org))!;
        }

        partial void OnIsModifiedChanged(bool value)
        {
            SaveCommand.NotifyCanExecuteChanged();
        }
    }
}
