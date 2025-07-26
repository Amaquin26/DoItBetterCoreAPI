using System.ComponentModel.DataAnnotations;

namespace DoItBetterCoreAPI.Dtos.TodoTask
{
    public class TodoTaskWriteDto
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string? Subtitle { get; set; }

        public int? GroupId { get; set; }

        public DateTime? EstimatedEndDate { get; set; }
    }
}
