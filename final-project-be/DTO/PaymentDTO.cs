namespace final_project_be.DTO
{
    public class PaymentDTO
    {
        public string? Name { set; get; }
        public IFormFile? Image { set; get; }
        public bool IsActive { set; get; } 
    }
}
