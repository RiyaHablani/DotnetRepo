using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DiseaseType { get; set; } = string.Empty;
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        [MaxLength(100)]
        public string? Manufacturer { get; set; }
        
        [MaxLength(50)]
        public string? DosageForm { get; set; } // Tablet, Syrup, Injection, etc.
        
        [MaxLength(100)]
        public string? DosageStrength { get; set; } // 500mg, 10ml, etc.
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
