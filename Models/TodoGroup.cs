using System.ComponentModel.DataAnnotations;

namespace DoItBetterCoreAPI.Models
{
    public class TodoGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public bool IsPublic { get; set; } = false;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModified { get; set; }

        public string? UserId { get; set; }

        //  Navigation Property
        public AppUser? User { get; set; }
    }
}
