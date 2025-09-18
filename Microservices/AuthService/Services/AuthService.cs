using AuthService.Models.DTOs;
using AuthService.Models.Entities;
using AuthService.Models.Enums;
using AuthService.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.FindAsync(u => u.Username == request.Username && u.IsActive);
            var userEntity = user.FirstOrDefault();

            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(request.Password, userEntity.PasswordHash))
            {
                return null;
            }

            return GenerateAuthResponse(userEntity);
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            // Check if username or email already exists
            var existingUser = await _userRepository.FindAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (existingUser.Any())
            {
                return null;
            }

            var user = new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);
            return GenerateAuthResponse(createdUser);
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSettings = _configuration.GetSection("JWT");
                var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-for-hospital-management-system-2024";
                var issuer = jwtSettings["Issuer"] ?? "HospitalManagementSystem";
                var audience = jwtSettings["Audience"] ?? "HospitalManagementSystem";

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await _userRepository.FindAsync(u => u.Username == username && u.IsActive);
            return users.FirstOrDefault();
        }

        private AuthResponse GenerateAuthResponse(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-for-hospital-management-system-2024";
            var issuer = jwtSettings["Issuer"] ?? "HospitalManagementSystem";
            var audience = jwtSettings["Audience"] ?? "HospitalManagementSystem";
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("Role", user.Role.ToString())
            };

            if (user.DoctorId.HasValue)
            {
                claims.Add(new Claim("DoctorId", user.DoctorId.Value.ToString()));
            }

            if (user.PatientId.HasValue)
            {
                claims.Add(new Claim("PatientId", user.PatientId.Value.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResponse
            {
                Token = tokenString,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                DoctorId = user.DoctorId,
                PatientId = user.PatientId,
                ExpiresAt = expiresAt
            };
        }
    }
}


