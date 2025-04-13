using Server.Data.Db;
using Server.Data.Entities;
using Server.Data.Repositories;
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Supplier> Suppliers => _supplierRepository ??= new SupplierRepository(_context);

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();
    }

}
