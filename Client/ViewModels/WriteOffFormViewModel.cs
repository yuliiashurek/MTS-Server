using Client.Services.ApiServices;
using Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Data;
using Server.Shared.DTOs;
using Server.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModels
{
    public partial class WriteOffFormViewModel : ObservableObject
    {
        private readonly MaterialMovementViewModel _parentViewModel;
        public MaterialMovementDto SourceMovement { get; }

        [ObservableProperty]
        private decimal quantity;

        [ObservableProperty]
        private DateTime movementDate;

        public WriteOffFormViewModel(MaterialMovementDto source, MaterialMovementViewModel parent)
        {
            SourceMovement = source;
            _parentViewModel = parent;
            Quantity = source.Quantity;
            MovementDate = DateTime.Today;
        }

        [RelayCommand]
        private async Task ConfirmWriteOff(Window window)
        {
            if (window is not WriteOffForm form) return;

            if (Quantity <= 0 || Quantity > SourceMovement.Quantity)
            {
                HandyControl.Controls.Growl.Error("Некоректна кількість для списання!");
                return;
            }

            if (string.IsNullOrWhiteSpace(form.RecipientNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(form.RecipientEdrpouTextBox.Text) ||
                string.IsNullOrWhiteSpace(form.RecipientAddressTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля отримувача.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var recipientApi = new RecipientApiService();
            Guid? recipientId = form.SelectedRecipientId;

            if (!recipientId.HasValue)
            {
                var created = await recipientApi.CreateAsync(new RecipientDto
                {
                    Name = form.RecipientNameTextBox.Text,
                    Edrpou = form.RecipientEdrpouTextBox.Text,
                    Address = form.RecipientAddressTextBox.Text,
                    ContactPerson = form.RecipientContactTextBox.Text
                });

                recipientId = created.Id;
            }

            var newWriteOff = new MaterialMovementDto
            {
                Id = Guid.NewGuid(),
                OrganizationId = SourceMovement.OrganizationId,
                MaterialItemId = SourceMovement.MaterialItemId,
                WarehouseId = SourceMovement.WarehouseId,
                MovementType = 1,
                Quantity = Quantity,
                PricePerUnit = SourceMovement.PricePerUnit,
                MovementDate = MovementDate,
                ExpirationDate = SourceMovement.ExpirationDate,
                BarcodeNumber = SourceMovement.BarcodeNumber,
                RecipientId = recipientId
            };

            _parentViewModel.AddMaterialMovementWriteOff(newWriteOff);

            window.DialogResult = true;
            window.Close();
        }

    }

}
