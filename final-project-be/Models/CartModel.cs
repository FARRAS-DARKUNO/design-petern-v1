namespace final_project_be.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public string? Image { get; set; }
        public string? Title { get; set; }
        public DateTime Schedule { get; set; }
        public int Price { get; set; }
        public string? Category { get; set; }
    }
}
