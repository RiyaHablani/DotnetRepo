using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;
using HospitalManagementSystem.Repositories;

namespace HospitalManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await GetUserByUsernameAsync(loginDto.Username);
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Login attempt failed for username: {Username}", loginDto.Username);
                    return null;
                }

                if (!await ValidatePasswordAsync(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password for username: {Username}", loginDto.Username);
                    return null;
                }

                var token = GenerateJwtToken(user);
                var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes());

                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role.ToString(),
                    ExpiresAt = expiresAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for username: {Username}", loginDto.Username);
                return null;
            }
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if username or email already exists
                var existingUser = await GetUserByUsernameAsync(registerDto.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed - username already exists: {Username}", registerDto.Username);
                    return null;
                }

                var users = await _userRepository.GetAllAsync();
                if (users.Any(u => u.Email == registerDto.Email))
                {
                    _logger.LogWarning("Registration failed - email already exists: {Email}", registerDto.Email);
                    return null;
                }

                // Parse role
                if (!Enum.TryParse<UserRole>(registerDto.Role, out var role))
                {
                    _logger.LogWarning("Registration failed - invalid role: {Role}", registerDto.Role);
                    return null;
                }

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                // Create new user
                var newUser = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    Role = role,
                    PatientId = registerDto.PatientId,
                    DoctorId = registerDto.DoctorId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(newUser);

                var token = GenerateJwtToken(createdUser);
                var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes());

                _logger.LogInformation("User {Username} registered successfully", createdUser.Username);

                return new LoginResponseDto
                {
                    Token = token,
                    Username = createdUser.Username,
                    Role = createdUser.Role.ToString(),
                    ExpiresAt = expiresAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", registerDto.Username);
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Username == username);
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-for-hospital-management-system-2024";
            var issuer = jwtSettings["Issuer"] ?? "HospitalManagementSystem";
            var audience = jwtSettings["Audience"] ?? "HospitalManagementSystem";

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            // Add patient or doctor ID claims if applicable
            if (user.PatientId.HasValue)
                claims.Add(new Claim("PatientId", user.PatientId.Value.ToString()));
            
            if (user.DoctorId.HasValue)
                claims.Add(new Claim("DoctorId", user.DoctorId.Value.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(GetJwtExpiryMinutes()),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidatePasswordAsync(string password, string hash)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hash));
        }

        private int GetJwtExpiryMinutes()
        {
            return _configuration.GetValue<int>("JWT:ExpiryMinutes", 60);
        }
    }
}
