using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;
using System.Security.Claims;

namespace Server.API.Controllers.Controllers
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

        /// <summary>
        /// Отримати список користувачів у поточній організації (доступно лише адміністратору).
        /// </summary>
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var users = await _userService.GetAllInOrganizationAsync(userId);
            return Ok(users);
        }

        /// <summary>
        /// Створити нового користувача в організації.
        /// </summary>
        /// <param name="dto">Дані нового користувача.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.CreateUserAsync(userId, dto);
            return result ? Ok() : BadRequest("Не вдалося створити користувача");
        }

        /// <summary>
        /// Видалити користувача з організації.
        /// </summary>
        /// <param name="id">ID користувача, якого потрібно видалити.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.DeleteUserAsync(userId, id);
            return result ? Ok() : BadRequest("Не вдалося видалити користувача");
        }

        /// <summary>
        /// Повторно надіслати запрошення користувачу.
        /// </summary>
        /// <param name="userIdToInvite">ID користувача, якому потрібно надіслати запрошення.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] Guid userIdToInvite)
        {
            var adminId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _userService.SendInviteAsync(adminId, userIdToInvite);
            return result ? Ok() : BadRequest("Не вдалося надіслати запрошення");
        }
    }

}
