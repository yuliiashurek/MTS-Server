using Microsoft.AspNetCore.Mvc;
using Server.API.Controllers.Base;
using Server.Shared.DTOs;
using Server.Core.Interfaces;

namespace Server.API.Controllers
{
    public class MeasurementUnitsController : BaseController<MeasurementUnitDto>
    {
        public MeasurementUnitsController(IBaseService<MeasurementUnitDto> service) : base(service) { }
    }
}
