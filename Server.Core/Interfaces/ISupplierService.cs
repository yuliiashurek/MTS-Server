using Server.Shared.DTOs;

namespace Server.Core.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(Guid id);
        Task AddAsync(SupplierDto supplierDto);
        Task UpdateAsync(SupplierDto supplierDto);
        Task DeleteAsync(Guid id);
    }
}
