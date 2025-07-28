using DoItBetterCoreAPI.Models;

namespace DoItBetterCoreAPI.Repositories
{
    public interface ITodoSubtaskRepository
    {
        Task<IEnumerable<TodoSubtask>> GetAllAsync();
        Task<IEnumerable<TodoSubtask>> GetAllByTodoTaskIdAsync(int taskId);
        Task<TodoSubtask?> GetByIdAsync(int id);
        Task<IEnumerable<bool>> GetAllSubtasksStatus(int taskId);
        Task AddAsync(TodoSubtask subtask);
        Task UpdateAsync(TodoSubtask subtask);
        Task DeleteAsync(TodoSubtask subtask);
        Task<bool> SaveChangesAsync();
    }
}
