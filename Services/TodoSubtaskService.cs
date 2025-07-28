using DoItBetterCoreAPI.Dtos.TodoSubtask;
using DoItBetterCoreAPI.Dtos.TodoTask;
using DoItBetterCoreAPI.Models;
using DoItBetterCoreAPI.Repositories;
using System.Threading.Tasks;

namespace DoItBetterCoreAPI.Services
{
    public class TodoSubtaskService : ITodoSubtaskService
    {
        private readonly ITodoSubtaskRepository _todoSubtaskRepository;
        private readonly ITodoTaskService _todoTaskService;

        public TodoSubtaskService(ITodoSubtaskRepository todoSubtaskRepository, ITodoTaskService todoTaskService)
        {
            _todoSubtaskRepository = todoSubtaskRepository;
            _todoTaskService = todoTaskService;
        }

        public async Task<IEnumerable<TodoSubtaskReadDto>> GetAllAsync(string userId)
        {
            var subtasks = await _todoSubtaskRepository.GetAllAsync();

            // TODO: Make this more performant
            var subtaskDtos = subtasks.Select(subtask =>
            new TodoSubtaskReadDto
            {
                Id = subtask.Id,
                TaskId = subtask.TodoTaskId,
                Name = subtask.Name,
                EndTime = subtask.EndTime,
                BeginTime = subtask.BeginTime,
                DateCreated = subtask.DateCreated,
                DateModified = subtask.DateModified,
                IsChecked = subtask.IsChecked,
                IsOwner = subtask.TodoTask != null ? subtask.TodoTask?.UserId == userId : false
            })
            .ToList();

            return subtaskDtos;
        }

        public async Task<IEnumerable<TodoSubtaskReadDto>> GetAllByTaskId(int taskId, string userId)
        {
            var subtasks = await _todoSubtaskRepository.GetAllByTodoTaskIdAsync(taskId);

            // TODO: Make this more performant
            var subtaskDtos = subtasks.Select(subtask =>
            new TodoSubtaskReadDto
            {
                Id = subtask.Id,
                TaskId = subtask.TodoTaskId,
                Name = subtask.Name,
                EndTime = subtask.EndTime,
                BeginTime = subtask.BeginTime,
                DateCreated = subtask.DateCreated,
                DateModified = subtask.DateModified,
                IsChecked = subtask.IsChecked,
                IsOwner = subtask.TodoTask != null ? subtask.TodoTask?.UserId == userId : false
            })
            .ToList();

            return subtaskDtos;
        }

        public async Task<TodoSubtaskReadDto?> GetByIdAsync(int id, string userId)
        {
            var subtask = await _todoSubtaskRepository.GetByIdAsync(id);

            if (subtask == null)
            {
                throw new KeyNotFoundException($"Subtask with ID {id} not found.");
            }

            var subtaskDto = new TodoSubtaskReadDto
            {
                Id = subtask.Id,
                TaskId = subtask.TodoTaskId,
                Name = subtask.Name,
                EndTime = subtask.EndTime,
                BeginTime = subtask.BeginTime,
                DateCreated = subtask.DateCreated,
                DateModified = subtask.DateModified,
                IsChecked = subtask.IsChecked,
                IsOwner = subtask.TodoTask != null ? subtask.TodoTask?.UserId == userId : false
            };

            return subtaskDto;
        }

        public async Task<int> AddAsync(TodoSubtaskWriteDto subtaskDto)
        {
            if (subtaskDto.TaskId == null)
            {
                throw new ArgumentNullException("Task ID must be provided to create a subtask.");
            }

            TodoSubtask subtask = new TodoSubtask
            {
                Name = subtaskDto.Name,
                EndTime = subtaskDto.EndTime,
                BeginTime = subtaskDto.BeginTime,
                TodoTaskId = subtaskDto.TaskId.Value,
            };

            await _todoSubtaskRepository.AddAsync(subtask);
            await _todoSubtaskRepository.SaveChangesAsync();

            var newProgress = await _todoTaskService.UpdateTaskProgressAsync(subtask.Id);

            return subtask.Id;
        }

        public async Task<int> CheckToggleAsync(int id, string userId)
        {
            var existingSubtask = await _todoSubtaskRepository.GetByIdAsync(id);

            if (existingSubtask == null)
            {
                throw new KeyNotFoundException($"Subtask with ID {id} not found.");
            }

            if (existingSubtask.TodoTask == null || existingSubtask.TodoTask.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to check or uncheck this Subtask.");
            }

            existingSubtask.IsChecked = !existingSubtask.IsChecked;

            await _todoSubtaskRepository.UpdateAsync(existingSubtask);
            await _todoSubtaskRepository.SaveChangesAsync();

            var newProgress = await _todoTaskService.UpdateTaskProgressAsync(existingSubtask.TodoTaskId);
            return newProgress;

        }

        public async Task UpdateAsync(TodoSubtaskWriteDto subtaskDto, string userId)
        {
            if (subtaskDto.Id == null)
            {
                throw new ArgumentNullException("Subtask ID must be provided to update.");
            }

            var existingSubtask = await _todoSubtaskRepository.GetByIdAsync(subtaskDto.Id.Value);

            if (existingSubtask == null)
            {
                throw new KeyNotFoundException($"Subtask with ID {subtaskDto.Id} not found.");
            }

            if (existingSubtask.TodoTask == null || existingSubtask.TodoTask.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this Subtask.");
            }

            existingSubtask.Name = subtaskDto.Name;
            existingSubtask.EndTime = subtaskDto.EndTime;
            existingSubtask.BeginTime = subtaskDto.BeginTime;
            existingSubtask.DateModified = DateTime.UtcNow;

            await _todoSubtaskRepository.UpdateAsync(existingSubtask);
            await _todoSubtaskRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var existingSubtask = await _todoSubtaskRepository.GetByIdAsync(id);

            if (existingSubtask == null)
            {
                throw new KeyNotFoundException($"Subtask with ID {id} not found.");
            }

            if (existingSubtask.TodoTask == null || existingSubtask.TodoTask.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this Subtask.");
            }

            await _todoSubtaskRepository.DeleteAsync(existingSubtask);
            await _todoSubtaskRepository.SaveChangesAsync();

            var newProgress = await _todoTaskService.UpdateTaskProgressAsync(existingSubtask.TodoTaskId);
        }
    }
}
