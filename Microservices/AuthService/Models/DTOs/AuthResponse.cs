using AuthService.Models.Enums;

namespace AuthService.Models.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}


