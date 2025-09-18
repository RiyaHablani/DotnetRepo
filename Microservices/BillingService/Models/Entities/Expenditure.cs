using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.Entities
{
    public class Expenditure
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty; // Equipment, Supplies, Utilities, etc.
        
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal Amount { get; set; }
        
        [MaxLength(100)]
        public string? Vendor { get; set; }
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [Required]
        public DateTime ExpenditureDate { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}


