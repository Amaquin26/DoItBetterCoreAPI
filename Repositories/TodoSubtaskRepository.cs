using DoItBetterCoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DoItBetterCoreAPI.Repositories
{
    public class TodoSubtaskRepository : ITodoSubtaskRepository
    {
        private readonly AppDbContext _context;

        public TodoSubtaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoSubtask>> GetAllAsync()
        {
            return await _context.TodoSubtasks
                .Include(t => t.TodoTask)
                .Where(t => !t.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<TodoSubtask>> GetAllByTodoTaskIdAsync(int taskId)
        {
            return await _context.TodoSubtasks
                .Include(t => t.TodoTask)
                .Where(t => !t.IsDeleted && t.TodoTaskId == taskId).ToListAsync();
        }

        public async Task<TodoSubtask?> GetByIdAsync(int id)
        {
            return await _context.TodoSubtasks
                .Include(t => t.TodoTask)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task AddAsync(TodoSubtask subtask)
        {
            await _context.TodoSubtasks.AddAsync(subtask);
        }

        public async Task UpdateAsync(TodoSubtask subtask)
        {
            _context.TodoSubtasks.Update(subtask);
        }

        public async Task DeleteAsync(TodoSubtask subtask)
        {
            subtask.IsDeleted = true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
