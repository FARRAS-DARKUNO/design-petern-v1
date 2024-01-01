namespace final_project_be.DTO
{
    public class InvoiceDTO
    {
        public int PaymentId { set; get;}
        public string? NoInvoice {set; get;}
        public List<InvoiceFromCart>? ListData { set; get; }
        public DateTime CreateAt { set; get; }
    }
}
