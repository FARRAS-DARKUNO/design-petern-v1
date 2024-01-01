namespace final_project_be.DTO
{
    public class CategoryDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
