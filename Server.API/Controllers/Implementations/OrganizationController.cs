using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers.Controllers
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

        /// <summary>
        /// Отримання інформації про організацію поточного користувача.
        /// </summary>
        [ProducesResponseType(typeof(OrganizationInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("me")]
        public async Task<ActionResult<OrganizationInfoDto>> GetMyOrganization()
        {
            var userId = _userSession.UserId;
            var result = await _organizationService.GetMyOrganizationAsync(userId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Оновлення інформації про організацію поточного користувача.
        /// </summary>
        /// <param name="dto">Оновлені дані організації.</param>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyOrganization([FromBody] OrganizationInfoDto dto)
        {
            var userId = _userSession.UserId;
            var success = await _organizationService.UpdateMyOrganizationAsync(userId, dto);
            return success ? NoContent() : NotFound();
        }
    }

}
