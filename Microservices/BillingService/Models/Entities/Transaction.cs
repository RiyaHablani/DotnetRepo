using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty; // Payment, Refund, Adjustment
        
        [Required]
        public decimal Amount { get; set; }
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; } // Cash, Card, Insurance, etc.
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [Required]
        public DateTime TransactionDate { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}


