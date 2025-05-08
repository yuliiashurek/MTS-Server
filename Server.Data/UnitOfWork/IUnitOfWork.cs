using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Supplier> Suppliers { get; }
        IRecipientRepository Recipients { get; }
        IRepository<Category> Categories { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<MeasurementUnit> MeasurementUnits { get; }
        IRepository<MaterialItem> MaterialItems { get; }
        IRepository<MaterialNotificationHistory> MaterialNotificationsHistory { get; }
        IRepository<MaterialMovement> MaterialMovements { get; }
        IUserRepository Users { get; }
        IRepository<Organization> Organizations { get; }

        Task<int> SaveChangesAsync();

    }

}
