using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        string GenerateJwtToken(User user);
        Task<bool> ValidatePasswordAsync(string password, string hash);
    }
}
