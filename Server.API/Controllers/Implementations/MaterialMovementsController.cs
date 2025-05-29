using Microsoft.AspNetCore.Mvc;
using Server.API.Controllers.Base;
using Server.Shared.DTOs;
using Server.Core.Interfaces;

namespace Server.API.Controllers.Controllers
{
    public class MaterialMovementsController : BaseController<MaterialMovementDto>
    {
        public MaterialMovementsController(IBaseService<MaterialMovementDto> service) : base(service) { }
    }
}
