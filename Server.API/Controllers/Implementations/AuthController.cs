using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Core.Interfaces;
using Server.Data.Entities;
using Server.Shared.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Авторизація користувача за логіном та паролем.
    /// </summary>
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.AuthenticateAsync(dto);
        return result is null ? Unauthorized() : Ok(result);
    }

    /// <summary>
    /// Оновлення access токена за допомогою refresh токена.
    /// </summary>
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var newToken = await _authService.RefreshTokenAsync(refreshToken);
        return newToken is null ? Unauthorized() : Ok(new TokenResponseDto { Token = newToken });
    }

    /// <summary>
    /// Реєстрація нової організації з першим адміністратором.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register-organization")]
    public async Task<IActionResult> RegisterOrganization([FromBody] RegisterOrganizationDto dto)
    {
        var success = await _authService.RegisterOrganizationAsync(dto);
        return success ? Ok("Організація зареєстрована") : BadRequest("Помилка реєстрації");
    }

    /// <summary>
    /// Запрошення нового користувача (доступно лише адміністратору).
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = "Admin")]
    [HttpPost("invite-user")]
    public async Task<IActionResult> InviteUser([FromBody] CreateUserDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var success = await _authService.InviteUserAsync(Guid.Parse(userId), dto);
        return success ? Ok("Користувача запрошено") : BadRequest("Помилка запрошення");
    }

    /// <summary>
    /// Встановлення пароля за допомогою токена запрошення.
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
    {
        var success = await _authService.SetPasswordAsync(dto);
        return success ? Ok("Пароль встановлено") : BadRequest("Недійсний або прострочений токен");
    }
}
