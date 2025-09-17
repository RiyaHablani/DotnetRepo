using System.ComponentModel.DataAnnotations;

namespace MedicalRecordsService.Models.Entities
{
    public class LabReport
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TestName { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? TestDescription { get; set; }
        
        [MaxLength(1000)]
        public string? Results { get; set; }
        
        [MaxLength(100)]
        public string? Status { get; set; } = "Pending"; // Pending, Completed, Abnormal
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        public DateTime TestDate { get; set; }
        
        public DateTime? CompletedDate { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
