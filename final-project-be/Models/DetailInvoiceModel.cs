namespace final_project_be.Models
{
    public class DetailInvoiceModel
    {
        public string? NoInvoice { get; set; }
        public DateTime? CreateAt { get; set; }
        public List<InvoiceListModel>? ListInvoice { get; set; }
    }
}
