using System.ComponentModel.DataAnnotations;

namespace DoItBetterCoreAPI.Dtos.TodoSubtask
{
    public class TodoSubtaskWriteDto
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int? TaskId { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
