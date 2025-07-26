using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoItBetterCoreAPI.Models
{
    public class TodoSubtask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; } = null!;

        [Required]
        public bool IsChecked { get; set; } = false;

        public DateTime? EndTime { get; set; }

        public DateTime? BeginTime{ get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModified { get; set; }

        public int TodoTaskId { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        // Navigation Property
        public TodoTask? TodoTask { get; set; }
    }
}
