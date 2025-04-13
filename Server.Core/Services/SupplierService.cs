// SupplierService.cs
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.UnitOfWork;

namespace Server.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _unitOfWork.Suppliers.GetAllAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Suppliers.GetByIdAsync(id);
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (existing is null) return;

            _unitOfWork.Suppliers.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}