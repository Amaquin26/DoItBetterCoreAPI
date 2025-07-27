namespace DoItBetterCoreAPI.Dtos.TodoSubtask
{
    public class TodoSubtaskReadDto
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime? EndTime { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public bool IsChecked { get; set; }

        public bool IsOwner { get; set; }
    }
}
