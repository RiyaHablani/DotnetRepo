namespace BillingService.Models.DTOs
{
    public class InvoiceSearchDto
    {
        public int? PatientId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}


