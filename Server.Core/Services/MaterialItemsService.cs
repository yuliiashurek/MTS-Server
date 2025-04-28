using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

public class MaterialItemsService : BaseService<MaterialItem, MaterialItemDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public MaterialItemsService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
    }

    protected override IRepository<MaterialItem> Repository => _unitOfWork.MaterialItems;
}
