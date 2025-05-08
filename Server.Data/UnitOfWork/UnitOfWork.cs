using Server.Data.Db;
using Server.Data.Entities;
using Server.Data.Repositories.Implementations;
using Server.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private SupplierRepository? _supplierRepository;
        private RecipientRepository? _recipientRepository;
        private CategoryRepository? _categoriesRepository;
        private WarehouseRepository? _warehousesRepository;
        private MeasurementUnitRepository? _measurementUnitRepository;
        private OrganizationRepository? _organizationRepository;
        private MaterialItemRepository? _materialItemRepository;
        private MaterialMovementRepository? _materialMovementRepository;
        private MaterialNotificationHistoryRepository? _materialNotificationHistoryRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
        }

        public IRepository<Supplier> Suppliers => _supplierRepository ??= new SupplierRepository(_context);
        public IRecipientRepository Recipients => _recipientRepository ??= new RecipientRepository(_context);
        public IRepository<Category> Categories => _categoriesRepository ??= new CategoryRepository(_context);
        public IRepository<Warehouse> Warehouses => _warehousesRepository ??= new WarehouseRepository(_context);
        public IRepository<MeasurementUnit> MeasurementUnits => _measurementUnitRepository ??= new MeasurementUnitRepository(_context);
        public IRepository<MaterialItem> MaterialItems => _materialItemRepository ??= new MaterialItemRepository(_context);
        public IRepository<MaterialNotificationHistory> MaterialNotificationsHistory => _materialNotificationHistoryRepository ??= new MaterialNotificationHistoryRepository(_context);
        public IRepository<Organization> Organizations => _organizationRepository ??= new OrganizationRepository(_context);
        public IUserRepository Users { get; }

        public IRepository<MaterialMovement> MaterialMovements => _materialMovementRepository ?? new MaterialMovementRepository(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }

}
