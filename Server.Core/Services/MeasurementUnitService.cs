using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

public class MeasurementUnitService : BaseService<MeasurementUnit, MeasurementUnitDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public MeasurementUnitService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
    }

    protected override IRepository<MeasurementUnit> Repository => _unitOfWork.MeasurementUnits;
}
