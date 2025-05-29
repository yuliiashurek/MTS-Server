using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Interfaces;
using Server.Shared.DTOs;

namespace Server.API.Controllers.Base
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseController<TDto> : Controller
        where TDto : class, IBaseDto
    {
        private readonly IBaseService<TDto> _service;

        protected BaseController(IBaseService<TDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Отримати список усіх елементів.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Отримати елемент за його ID.
        /// </summary>
        /// <param name="id">Ідентифікатор елемента.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Створити новий елемент і повернути його.
        /// </summary>
        /// <param name="dto">Дані нового елемента.</param>
        [HttpPost("create-and-return")]
        public async Task<ActionResult<TDto>> CreateAndReturn([FromBody] TDto dto)
        {
            var createdDto = await _service.AddAndReturnAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        /// <summary>
        /// Створити новий елемент.
        /// </summary>
        /// <param name="dto">Дані нового елемента.</param>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Оновити існуючий елемент.
        /// </summary>
        /// <param name="id">ID елемента.</param>
        /// <param name="dto">Оновлені дані.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            await _service.UpdateAsync(dto);
            return Ok();
        }

        /// <summary>
        /// Видалити елемент за ID.
        /// </summary>
        /// <param name="id">ID елемента.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
