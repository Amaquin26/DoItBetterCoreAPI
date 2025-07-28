using DoItBetterCoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DoItBetterCoreAPI.Repositories
{
    public class TodoTaskRepository: ITodoTaskRepository
    {
        private readonly AppDbContext _context;

        public TodoTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            return await _context.TodoTasks
                .Include(t => t.Group)
                .Include(t => t.User)
                .Where(t => !t.IsDeleted).ToListAsync();
        }

        public async Task<TodoTask?> GetByIdAsync(int id)
        {
            return await _context.TodoTasks
                .Include(t => t.Group)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<IEnumerable<TodoTask>> GetByUserIdAsync(string userId)
        {
            return await _context.TodoTasks
                                 .Where(t => !t.IsDeleted && t.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<TodoTask?> GetTodoTaskWithSubtasks(int taskId)
        {
            return await _context.TodoTasks
                .Include(t => t.SubTasks)
                .FirstOrDefaultAsync(s => s.Id == taskId);
        }

        public async Task AddAsync(TodoTask task)
        {
            await _context.TodoTasks.AddAsync(task);
        }

        public async Task UpdateAsync(TodoTask task)
        {
            _context.TodoTasks.Update(task);
        }

        public async Task DeleteAsync(TodoTask task)
        {
            task.IsDeleted = true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
