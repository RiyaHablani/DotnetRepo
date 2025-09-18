using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class UpdateExpenditureDto
    {
        [MaxLength(100)]
        public string? Category { get; set; }
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }
        
        [MaxLength(100)]
        public string? Vendor { get; set; }
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
        
        public DateTime? ExpenditureDate { get; set; }
    }
}


