using Microsoft.AspNetCore.Mvc;
using Server.API.Controllers.Base;
using Server.Shared.DTOs;
using Server.Core.Interfaces;

namespace Server.API.Controllers.Controllers
{
    public class CategoriesController : BaseController<CategoryDto>
    {
        public CategoriesController(IBaseService<CategoryDto> service) : base(service) { }
    }
}
