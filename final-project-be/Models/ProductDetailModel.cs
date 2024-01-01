namespace final_project_be.Models
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public List<ScheduleModel>? Schedules { get; set; }
    }
}
