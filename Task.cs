namespace TODOApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public bool TaskStatus { get; set; }
        public int TaskLimit { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
