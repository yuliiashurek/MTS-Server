using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Server.Shared.Enums;

public class MaterialMovementsService : BaseService<MaterialMovement, MaterialMovementDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _session;
    private readonly IEmailService _emailService;

    public MaterialMovementsService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session, IEmailService emailService)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _session = session;
        _emailService = emailService;
    }

    protected override IRepository<MaterialMovement> Repository => _unitOfWork.MaterialMovements;

    public override async Task<MaterialMovementDto> AddAndReturnAsync(MaterialMovementDto dto)
    {
        var entity = _mapper.Map<MaterialMovement>(dto);

        entity.OrganizationId = _session.OrganizationId;

        entity.BarcodeNumber = await GenerateBarcodeNumberAsync(entity.OrganizationId);

        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<MaterialMovementDto>(entity);
    }

    public override async Task<MaterialMovementDto> AddAsync(MaterialMovementDto dto)
    {
        var entity = _mapper.Map<MaterialMovement>(dto);

        entity.OrganizationId = _session.OrganizationId;

        if (string.IsNullOrEmpty(dto.BarcodeNumber))
        {
            entity.BarcodeNumber = await GenerateBarcodeNumberAsync(entity.OrganizationId);
        }

        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        if (entity.MovementType == (int)MovementType.Out)
        {
            var stock = await GetCurrentStockAsync(entity.MaterialItemId);

            var allItems = await _unitOfWork.MaterialItems.GetAllAsync();
            var material = allItems.FirstOrDefault(m => m.Id == entity.MaterialItemId);


            if (material != null && stock <= material.MinimumStock)
            {
                var emails = material.GetNotificationEmails();

                foreach (var email in emails)
                {
                    await _emailService.SendLowStockAlert(
                        email,
                        material.Name,
                        stock,
                        material.MinimumStock);
                }

            }
        }


        return _mapper.Map<MaterialMovementDto>(entity);
    }


    private async Task<string> GenerateBarcodeNumberAsync(Guid organizationId)
    {
        int count = (await Repository.GetAllAsync())
    .Where(x => x.OrganizationId == organizationId)
    .Count();


        return $"MM-{count + 1:D6}";
    }


    private async Task<decimal> GetCurrentStockAsync(Guid materialItemId)
    {
        var allMovements = await Repository.GetAllAsync();

        var movements = allMovements
            .Where(m => m.MaterialItemId == materialItemId)
            .ToList();

        return movements.Sum(m => m.MovementType == (int)MovementType.In ? m.Quantity : -m.Quantity);

    }

}
