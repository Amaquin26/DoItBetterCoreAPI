using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoItBetterCoreAPI.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<TodoTask> TodoTasks { get; set; }

        public DbSet<TodoSubtask> TodoSubtasks { get; set; }

        public DbSet<TodoGroup> TodoGroups { get; set; }
    }
}
