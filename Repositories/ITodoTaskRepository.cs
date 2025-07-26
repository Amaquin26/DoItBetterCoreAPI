using DoItBetterCoreAPI.Models;

namespace DoItBetterCoreAPI.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<IEnumerable<TodoTask>> GetAllAsync();
        Task<TodoTask?> GetByIdAsync(int id);
        Task<IEnumerable<TodoTask>> GetByUserIdAsync(string userId);
        Task AddAsync(TodoTask task);
        Task UpdateAsync(TodoTask task);
        Task DeleteAsync(TodoTask task);
        Task<bool> SaveChangesAsync();
    }
}
