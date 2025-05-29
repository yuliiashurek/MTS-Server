using Microsoft.AspNetCore.Mvc;
using Server.API.Controllers.Base;
using Server.Shared.DTOs;
using Server.Core.Interfaces;

namespace Server.API.Controllers.Controllers
{
    public class WarehousesController : BaseController<WarehouseDto>
    {
        public WarehousesController(IBaseService<WarehouseDto> service) : base(service) { }
    }
}
