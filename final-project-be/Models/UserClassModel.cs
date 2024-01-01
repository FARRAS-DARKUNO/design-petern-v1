namespace final_project_be.Models
{
    public class UserClassModel
    {
        public int IdSchedule { get; set; }
        public int ProductId { get; set; }
        public string? Image { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public DateTime Schedule { get; set; }
    }
}
