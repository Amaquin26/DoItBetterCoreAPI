using DoItBetterCoreAPI.Dtos.TodoTask;
using DoItBetterCoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoItBetterCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;

        public TodoTaskController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        private string UserId => User.FindFirst("userID")?.Value!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTaskReadDto>>> GetAll()
        {
            var tasks = await _todoTaskService.GetAllAsync(UserId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTaskReadDto>> GetById(int id)
        {
            var task = await _todoTaskService.GetByIdAsync(id, UserId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TodoTaskWriteDto taskDto)
        {
            var task = await _todoTaskService.AddAsync(taskDto, UserId);
            return Ok(task);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TodoTaskWriteDto taskDto)
        {
            await _todoTaskService.UpdateAsync(taskDto, UserId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoTaskService.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}
