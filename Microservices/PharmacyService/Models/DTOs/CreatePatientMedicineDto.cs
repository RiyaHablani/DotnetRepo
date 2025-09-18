using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class CreatePatientMedicineDto
    {
        [Required]
        public int MedicineId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        [MaxLength(200)]
        public string? Instructions { get; set; }
        
        [MaxLength(100)]
        public string? Dosage { get; set; }
        
        [MaxLength(50)]
        public string? Frequency { get; set; }
        
        [MaxLength(50)]
        public string? Duration { get; set; }
    }
}

