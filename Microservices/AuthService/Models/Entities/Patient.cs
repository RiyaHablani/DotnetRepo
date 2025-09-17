using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public int Age { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? Address { get; set; }
        
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        
        [MaxLength(20)]
        public string? EmergencyContact { get; set; }
        
        [MaxLength(200)]
        public string? MedicalHistory { get; set; }
        
        [MaxLength(200)]
        public string? Allergies { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
