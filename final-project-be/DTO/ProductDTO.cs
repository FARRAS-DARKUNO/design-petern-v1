namespace final_project_be.DTO
{
    public class ProductDTO
    {
        public string? TitleProduct { get; set; }
        public string? DescriptionProduct { get; set;}
        public int? CategoryId { get; set;}
        public int? Price { get; set;}
        public IFormFile? Image { get; set;}
        public bool? IsActive { get; set;}
    }
}
