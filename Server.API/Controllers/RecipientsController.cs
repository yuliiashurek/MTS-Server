using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipientsController : ControllerBase
    {
        private readonly IRecipientService _service;

        public RecipientsController(IRecipientService service)
        {
            _service = service;
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<RecipientDto?>> GetByName(string name)
        {
            var recipient = await _service.GetByNameAsync(name);
            return Ok(recipient);
        }

        [HttpPost]
        public async Task<ActionResult<RecipientDto>> Create([FromBody] RecipientDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
    }

}
