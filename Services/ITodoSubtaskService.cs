using DoItBetterCoreAPI.Dtos.TodoSubtask;
using DoItBetterCoreAPI.Dtos.TodoTask;

namespace DoItBetterCoreAPI.Services
{
    public interface ITodoSubtaskService
    {
        Task<IEnumerable<TodoSubtaskReadDto>> GetAllAsync(string userId);
        Task<IEnumerable<TodoSubtaskReadDto>> GetAllByTaskId(int taskId, string userId);
        Task<TodoSubtaskReadDto?> GetByIdAsync(int id, string userId);
        Task<int> CheckToggleAsync(int id, string userId);
        Task<int> AddAsync(TodoSubtaskWriteDto subtaskDto);
        Task UpdateAsync(TodoSubtaskWriteDto subtaskDto, string userId);
        Task DeleteAsync(int id, string userId);
    }
}
