using AuthService.Models.DTOs;
using AuthService.Models.Entities;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        Task<bool> ValidateTokenAsync(string token);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
