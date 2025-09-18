namespace BillingService.Models.DTOs
{
    public class TransactionSearchDto
    {
        public int? PatientId { get; set; }
        public string? TransactionType { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
    }
}


