namespace BillingService.Models.DTOs
{
    public class ExpenditureDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Vendor { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime ExpenditureDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


