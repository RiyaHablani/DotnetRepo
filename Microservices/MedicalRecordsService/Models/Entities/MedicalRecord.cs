using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsService.Models.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Diagnosis { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Symptoms { get; set; }
        
        [MaxLength(1000)]
        public string? Treatment { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
        
        [Required]
        public DateTime RecordDate { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
