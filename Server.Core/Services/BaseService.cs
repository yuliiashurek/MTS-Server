using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;

public abstract class BaseService<TEntity, TDto> : IBaseService<TDto>
    where TEntity : class, IOrganizationOwnedEntity, new()
    where TDto : class, IBaseDto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISessionService _session;

    protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _session = session;
    }

    protected abstract IRepository<TEntity> Repository { get; }

    public async Task<List<TDto>> GetAllAsync()
    {
        var orgId = _session.OrganizationId;
        var all = await Repository.GetAllAsync();
        return _mapper.Map<List<TDto>>(all.Where(e => e.OrganizationId == orgId));
    }

    public async Task<TDto?> GetByIdAsync(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<TDto>(entity);
    }

    public async virtual Task<TDto> AddAndReturnAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        entity.OrganizationId = _session.OrganizationId;

        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TDto>(entity);
    }

    public async virtual Task AddAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        entity.OrganizationId = _session.OrganizationId;
        await Repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }


    public async Task UpdateAsync(TDto dto)
    {
        var existing = await Repository.GetByIdAsync(dto.Id);
        if (existing == null) return;

        _mapper.Map(dto, existing);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await Repository.GetByIdAsync(id);
        if (existing == null) return;

        Repository.Remove(existing);
        await _unitOfWork.SaveChangesAsync();
    }
}
