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

        [HttpGet]
        public async Task<ActionResult<List<TDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("create-and-return")]
        public async Task<ActionResult<TDto>> CreateAndReturn([FromBody] TDto dto)
        {
            var createdDto = await _service.AddAndReturnAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            await _service.AddAsync(dto);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
