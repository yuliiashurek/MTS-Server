using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private void ConfirmWriteOff(Window window)
        {
            if (Quantity <= 0 || Quantity > SourceMovement.Quantity)
            {
                MessageBox.Show("Некоректна кількість для списання!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
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
                BarcodeNumber = SourceMovement.BarcodeNumber
            };

            // додай в БД
            _parentViewModel.AddMaterialMovementWriteOff(newWriteOff);

            window.DialogResult = true;
            window.Close();
        }
    }

}
