using Microsoft.AspNetCore.Mvc;
using AuthService.Models.DTOs;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                if (response == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                if (response == null)
                {
                    return BadRequest(new { message = "Username or email already exists" });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult TestAuth()
        {
            return Ok(new { message = "Auth service is working", timestamp = DateTime.UtcNow });
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var username = User.Identity?.Name;
            var role = User.FindFirst("Role")?.Value;
            var doctorId = User.FindFirst("DoctorId")?.Value;
            var patientId = User.FindFirst("PatientId")?.Value;

            return Ok(new
            {
                message = "Token is valid",
                username,
                role,
                doctorId,
                patientId,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
