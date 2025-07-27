using DoItBetterCoreAPI.Dtos.TodoTask;
using DoItBetterCoreAPI.Models;
using DoItBetterCoreAPI.Repositories;
using System.Threading.Tasks;

namespace DoItBetterCoreAPI.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _todoTaskRepository;

        public TodoTaskService(ITodoTaskRepository todoTaskRepository)
        {
            _todoTaskRepository = todoTaskRepository;
        }

        public async Task<IEnumerable<TodoTaskReadDto>> GetAllAsync(string userId)
        {
            var tasks = await _todoTaskRepository.GetAllAsync();

            // TODO: Make this more performant
            var taskDtos = tasks.Select(task => 
            new TodoTaskReadDto {
                Id = task.Id,
                Title = task.Title,
                Subtitle = task.Subtitle,
                Status = task.Status,
                Group = task.Group?.Name,
                DateCreated = task.DateCreated,
                DateModified = task.DateModified,
                EstimatedEndDate = task.EstimatedEndDate,
                Progress = task.Progress,
                TaskOwner = task.User?.FirstName + " "  + task.User?.LastName,
                GroupId = task.GroupId,
                IsOwner = task.UserId == userId
            })
            .ToList();

            return taskDtos;
        }

        public async Task<IEnumerable<TodoTaskReadDto>> GetAllUserOwnedAsync(string userId)
        {
            var tasks = await _todoTaskRepository.GetAllAsync();

            // TODO: Make this more performant
            var taskDtos = tasks
            .Where(t => t.UserId == userId)    
            .Select(task =>
            new TodoTaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Subtitle = task.Subtitle,
                Status = task.Status,
                Group = task.Group?.Name,
                DateCreated = task.DateCreated,
                DateModified = task.DateModified,
                EstimatedEndDate = task.EstimatedEndDate,
                Progress = task.Progress,
                TaskOwner = task.User?.FirstName + " " + task.User?.LastName,
                GroupId = task.GroupId,
                IsOwner = task.UserId == userId
            })
            .ToList();

            return taskDtos;
        }

        public async Task<TodoTaskReadDto?> GetByIdAsync(int id, string userId)
        {
            var task = await _todoTaskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            var taskDto = new TodoTaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Subtitle = task.Subtitle,
                Status = task.Status,
                Group = task.Group?.Name,
                DateCreated = task.DateCreated,
                DateModified = task.DateModified,
                EstimatedEndDate = task.EstimatedEndDate,
                Progress = task.Progress,
                TaskOwner = task.User?.FirstName + " " + task.User?.LastName,
                GroupId = task.GroupId,
                IsOwner = task.UserId == userId
            };

            return taskDto;
        }

        public async Task<int> AddAsync(TodoTaskWriteDto taskDto, string userId)
        {
            TodoTask task = new TodoTask
            {
                Title = taskDto.Title,
                Subtitle = taskDto.Subtitle,
                Status = "On Going",
                GroupId = taskDto.GroupId,
                EstimatedEndDate = taskDto.EstimatedEndDate,
                Progress = 0,
                UserId = userId
            };

            await _todoTaskRepository.AddAsync(task);
            await _todoTaskRepository.SaveChangesAsync();
            return task.Id;
        }

        public async Task UpdateAsync(TodoTaskWriteDto taskDto, string userId)
        {
            if (taskDto.Id == null)
            {
                throw new ArgumentNullException("Task ID must be provided for update.");
            }

            var existingTask = await _todoTaskRepository.GetByIdAsync(taskDto.Id.Value);

            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskDto.Id} not found.");
            }

            if (existingTask.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            existingTask.Title = taskDto.Title;
            existingTask.Subtitle = taskDto.Subtitle;
            existingTask.GroupId = taskDto.GroupId;
            existingTask.GroupId = taskDto.GroupId;
            existingTask.EstimatedEndDate = taskDto.EstimatedEndDate;
            existingTask.DateModified = DateTime.UtcNow;

            await _todoTaskRepository.UpdateAsync(existingTask);
            await _todoTaskRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var existingTask = await _todoTaskRepository.GetByIdAsync(id);

            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            if (existingTask.UserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this task.");
            }

            await _todoTaskRepository.DeleteAsync(existingTask);
            await _todoTaskRepository.SaveChangesAsync();
        }
    }
}
