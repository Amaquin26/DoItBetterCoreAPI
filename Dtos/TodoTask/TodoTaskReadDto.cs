namespace DoItBetterCoreAPI.Dtos.TodoTask
{
    public class TodoTaskReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Subtitle { get; set; }

        public string Status { get; set; } = null!;

        public string? Group { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public DateTime? EstimatedEndDate { get; set; }

        public int Progress { get; set; }

        public string TaskOwner { get; set; } = null!;  

        public int? GroupId { get; set; } = null!;  

        public bool IsOwner { get; set; }
    }
}
