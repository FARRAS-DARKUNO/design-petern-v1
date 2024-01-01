namespace final_project_be.Models
{
    public class InvoiceModel
    {
        public string? InvoiceNumber { get; set; }    
        public DateTime? CreatedAt { get; set; }
        public int TotalProduct { get; set; }
        public int TotalPrice { get; set; }
        
    }
}
