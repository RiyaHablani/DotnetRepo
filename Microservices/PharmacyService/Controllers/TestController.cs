using Microsoft.AspNetCore.Mvc;

namespace PharmacyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "PharmacyService is running successfully!",
                timestamp = DateTime.UtcNow,
                service = "PharmacyService",
                version = "1.0.0",
                status = "healthy"
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "healthy",
                service = "PharmacyService",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
