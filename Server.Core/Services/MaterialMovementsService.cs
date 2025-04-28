using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

public class MaterialMovementsService : BaseService<MaterialMovement, MaterialMovementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _session;

    public MaterialMovementsService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _session = session;
    }

    protected override IRepository<MaterialMovement> Repository => _unitOfWork.MaterialMovements;

    public override async Task<MaterialMovementDto> AddAndReturnAsync(MaterialMovementDto dto)
    {
        var entity = _mapper.Map<MaterialMovement>(dto);

        // Ставимо organizationId
        entity.OrganizationId = _session.OrganizationId;

        // Генеруємо короткий номер для штрихкоду
        entity.BarcodeNumber = await GenerateBarcodeNumberAsync(entity.OrganizationId);

        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // Повертаємо новостворений об'єкт назад
        return _mapper.Map<MaterialMovementDto>(entity);
    }

    public override async Task<MaterialMovementDto> AddAsync(MaterialMovementDto dto)
    {
        var entity = _mapper.Map<MaterialMovement>(dto);

        // Ставимо organizationId
        entity.OrganizationId = _session.OrganizationId;

        // Генеруємо короткий номер для штрихкоду
        entity.BarcodeNumber = await GenerateBarcodeNumberAsync(entity.OrganizationId);

        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // Повертаємо новостворений об'єкт назад
        return _mapper.Map<MaterialMovementDto>(entity);
    }


    private async Task<string> GenerateBarcodeNumberAsync(Guid organizationId)
    {
        int count = (await Repository.GetAllAsync())
    .Where(x => x.OrganizationId == organizationId)
    .Count();


        return $"MM-{count + 1:D6}";
    }
}
