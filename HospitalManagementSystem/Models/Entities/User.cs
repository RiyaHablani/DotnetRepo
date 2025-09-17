using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;
        
        [Required]
        public UserRole Role { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties for linking to domain entities
        public int? PatientId { get; set; }
        public Patient? Patient { get; set; }
        
        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
    
    public enum UserRole
    {
        Admin,
        Doctor,
        Patient,
        Pharmacist,
        Finance
    }
}
