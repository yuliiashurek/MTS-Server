using Microsoft.AspNetCore.Mvc;
using Server.API.Controllers.Base;
using Server.Shared.DTOs;
using Server.Core.Interfaces;

namespace Server.API.Controllers.Controllers
{
    public class MaterialItemsController : BaseController<MaterialItemDto>
    {
        public MaterialItemsController(IBaseService<MaterialItemDto> service) : base(service) { }
    }
}
