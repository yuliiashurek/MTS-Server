using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly ISessionService _userSession;

        public OrganizationController(IOrganizationService organizationService, ISessionService userSession)
        {
            _organizationService = organizationService;
            _userSession = userSession;
        }

        [HttpGet("me")]
        public async Task<ActionResult<OrganizationInfoDto>> GetMyOrganization()
        {
            var userId = _userSession.UserId;
            var result = await _organizationService.GetMyOrganizationAsync(userId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyOrganization([FromBody] OrganizationInfoDto dto)
        {
            var userId = _userSession.UserId;
            var success = await _organizationService.UpdateMyOrganizationAsync(userId, dto);
            return success ? NoContent() : NotFound();
        }
    }

}
