using AutoMapper;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Data.Repositories.Interfaces;
using Server.Data.UnitOfWork;
using Server.Shared.DTOs;

public class CategoryService : BaseService<Category, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ISessionService session)
        : base(unitOfWork, mapper, session)
    {
        _unitOfWork = unitOfWork;
    }

    protected override IRepository<Category> Repository => _unitOfWork.Categories;
}
