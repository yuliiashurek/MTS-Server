using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

public class WarehouseService : BaseService<Warehouse, WarehouseDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
    }

    protected override IRepository<Warehouse> Repository => _unitOfWork.Warehouses;
}
