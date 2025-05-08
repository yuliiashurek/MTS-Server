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

        [HttpGet]
        public async Task<ActionResult<List<RecipientDto>>> GetAll()
        {
            var recipients = await _service.GetAllAsync();
            return Ok(recipients);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientDto?>> GetById(Guid id)
        {
            var recipient = await _service.GetByIdAsync(id);
            return recipient is null ? NotFound() : Ok(recipient);
        }

    }
}
