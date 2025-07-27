using DoItBetterCoreAPI.Dtos.TodoSubtask;
using DoItBetterCoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoItBetterCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class SubtaskController : ControllerBase
    {
        private readonly ITodoSubtaskService _todoSubtaskService;

        public SubtaskController(ITodoSubtaskService todoSubtaskService)
        {
            _todoSubtaskService = todoSubtaskService;
        }

        private string UserId => User.FindFirst("userID")?.Value!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoSubtaskReadDto>>> GetAll()
        {
            var subtasks = await _todoSubtaskService.GetAllAsync(UserId);
            return Ok(subtasks);
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<IEnumerable<TodoSubtaskReadDto>>> GetAllByTaskId(int taskId)
        {
            var subtasks = await _todoSubtaskService.GetAllByTaskId(taskId, UserId);
            return Ok(subtasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoSubtaskReadDto>> GetById(int id)
        {
            var subtask = await _todoSubtaskService.GetByIdAsync(id, UserId);
            if (subtask == null)
                return NotFound();

            return Ok(subtask);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ToggleCheck(int id)
        {
            await _todoSubtaskService.CheckToggleAsync(id, UserId);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] TodoSubtaskWriteDto dto)
        {
            var newId = await _todoSubtaskService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TodoSubtaskWriteDto dto)
        {
            await _todoSubtaskService.UpdateAsync(dto, UserId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoSubtaskService.DeleteAsync(id, UserId);
            return NoContent();
        }

    }
}
