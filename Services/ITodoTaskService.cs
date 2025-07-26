using DoItBetterCoreAPI.Dtos.TodoTask;
using DoItBetterCoreAPI.Models;

namespace DoItBetterCoreAPI.Services
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTaskReadDto>> GetAllAsync(string userId);
        Task<TodoTaskReadDto?> GetByIdAsync(int id, string userId);
        Task<int> AddAsync(TodoTaskWriteDto task, string userId);
        Task UpdateAsync(TodoTaskWriteDto task, string userId);
        Task DeleteAsync(int id, string userId);
    }
}
