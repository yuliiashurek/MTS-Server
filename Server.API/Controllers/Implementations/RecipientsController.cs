using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers.Controllers
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

        /// <summary>
        /// Отримати список усіх одержувачів.
        /// </summary>
        [ProducesResponseType(typeof(List<RecipientDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<List<RecipientDto>>> GetAll()
        {
            var recipients = await _service.GetAllAsync();
            return Ok(recipients);
        }

        /// <summary>
        /// Отримати одержувача за ім'ям.
        /// </summary>
        /// <param name="name">Ім’я одержувача.</param>
        [ProducesResponseType(typeof(RecipientDto), StatusCodes.Status200OK)]
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<RecipientDto?>> GetByName(string name)
        {
            var recipient = await _service.GetByNameAsync(name);
            return Ok(recipient);
        }

        /// <summary>
        /// Створити нового одержувача.
        /// </summary>
        /// <param name="dto">Дані одержувача.</param>
        [ProducesResponseType(typeof(RecipientDto), StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<ActionResult<RecipientDto>> Create([FromBody] RecipientDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Отримати одержувача за ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор одержувача.</param>
        [ProducesResponseType(typeof(RecipientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientDto?>> GetById(Guid id)
        {
            var recipient = await _service.GetByIdAsync(id);
            return recipient is null ? NotFound() : Ok(recipient);
        }

    }
}
