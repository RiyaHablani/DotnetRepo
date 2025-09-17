using System.ComponentModel.DataAnnotations;
using AuthService.Models.Enums;

namespace AuthService.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; }
        
        public int? DoctorId { get; set; }
        
        public int? PatientId { get; set; }
    }
}
