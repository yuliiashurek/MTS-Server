using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Server.Core.Interfaces;

namespace Server.Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid OrganizationId =>
            Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst("organizationId")?.Value!);

        public Guid UserId =>
            Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        public string Role =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value!;
    }
}
