using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;
using System.Security.Claims;

namespace Server.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var users = await _userService.GetAllInOrganizationAsync(userId);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.CreateUserAsync(userId, dto);
            return result ? Ok() : BadRequest("Не вдалося створити користувача");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.DeleteUserAsync(userId, id);
            return result ? Ok() : BadRequest("Не вдалося видалити користувача");
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] Guid userIdToInvite)
        {
            var adminId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.SendInviteAsync(adminId, userIdToInvite);
            return result ? Ok() : BadRequest("Не вдалося надіслати запрошення");
        }
    }

}
