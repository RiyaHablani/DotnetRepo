using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            var token = ExtractTokenFromHeader(context);

            if (!string.IsNullOrEmpty(token))
            {
                await AttachUserToContext(context, authService, token);
            }

            await _next(context);
        }

        private string? ExtractTokenFromHeader(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        private async Task AttachUserToContext(HttpContext context, IAuthService authService, string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JWT");
                var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-for-hospital-management-system-2024";
                var issuer = jwtSettings["Issuer"] ?? "HospitalManagementSystem";
                var audience = jwtSettings["Audience"] ?? "HospitalManagementSystem";

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    var user = await authService.GetUserByIdAsync(userId);
                    if (user != null && user.IsActive)
                    {
                        context.Items["User"] = user;
                        context.Items["UserId"] = userId;
                        context.Items["UserRole"] = user.Role.ToString();
                        
                        if (user.PatientId.HasValue)
                            context.Items["PatientId"] = user.PatientId.Value;
                        
                        if (user.DoctorId.HasValue)
                            context.Items["DoctorId"] = user.DoctorId.Value;
                    }
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                _logger.LogWarning("JWT token expired: {Message}", ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning("Invalid JWT token: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing JWT token");
            }
        }
    }
}
