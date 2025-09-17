using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public decimal PaidAmount { get; set; } = 0;
        
        [Required]
        public decimal BalanceAmount { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue, Cancelled
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime InvoiceDate { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? PaidDate { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
