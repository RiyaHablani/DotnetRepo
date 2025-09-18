using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class UpdateMedicineDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DiseaseType { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get; set; }
        
        [Required]
        public DateTime ExpiryDate { get; set; }
        
        [MaxLength(100)]
        public string? Manufacturer { get; set; }
        
        [MaxLength(50)]
        public string? DosageForm { get; set; }
        
        [MaxLength(100)]
        public string? DosageStrength { get; set; }
    }
}
