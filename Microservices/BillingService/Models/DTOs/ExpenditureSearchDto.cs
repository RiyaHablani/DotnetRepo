namespace BillingService.Models.DTOs
{
    public class ExpenditureSearchDto
    {
        public string? Category { get; set; }
        public string? Vendor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}


