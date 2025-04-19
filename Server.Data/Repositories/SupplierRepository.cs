using Server.Data.Db;
using Server.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Server.Data.Repositories
{
    public class SupplierRepository : IRepository<Supplier>
    {
        private readonly AppDbContext _context;
        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier?> GetByIdAsync(Guid id)
            => await _context.Suppliers.FindAsync(id);

        public async Task<IEnumerable<Supplier>> GetAllAsync()
            => await _context.Suppliers.ToListAsync();

        public async Task AddAsync(Supplier entity)
            => await _context.Suppliers.AddAsync(entity);

        public void Update(Supplier entity)
            => _context.Suppliers.Update(entity);

        public void Remove(Supplier entity)
            => _context.Suppliers.Remove(entity);
    }

}
