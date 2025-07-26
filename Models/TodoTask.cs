using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoItBetterCoreAPI.Models
{
    public class TodoTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(255)]
        public string? Subtitle { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = null!;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModified { get; set; }

        public DateTime? EstimatedEndDate { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; }

        public string UserId { get; set; } = null!;

        public int? GroupId { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        //  Navigation Property
        public AppUser? User { get; set; }

        public TodoGroup? Group { get; set; }

        public ICollection<TodoSubtask> SubTasks { get; set; } = new List<TodoSubtask>();
    }
}
