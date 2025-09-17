using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple mock login
            if (request.Username == "admin" && request.Password == "admin123")
            {
                return Ok(new { Token = "mock-jwt-token", Username = "admin", Role = "Admin" });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Simple mock registration
            return Ok(new { Token = "mock-jwt-token", Username = request.Username, Role = request.Role });
        }

        [HttpGet("test")]
        public IActionResult TestAuth()
        {
            return Ok(new { message = "Auth service is working" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
