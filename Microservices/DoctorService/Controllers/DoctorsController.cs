using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorService.Data;

namespace DoctorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public DoctorsController(HospitalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _context.Doctors
                .Where(d => d.IsActive)
                .Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Email = $"dr.{d.Name.Split(' ').Last().ToLower()}@hospital.com"
                })
                .ToListAsync();
            
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Where(d => d.Id == id && d.IsActive)
                .Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Email = $"dr.{d.Name.Split(' ').Last().ToLower()}@hospital.com"
                })
                .FirstOrDefaultAsync();
                
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
        {
            var doctor = new Doctor
            {
                Name = request.Name,
                Specialization = request.Specialization,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var response = new DoctorResponse
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Email = $"dr.{doctor.Name.Split(' ').Last().ToLower()}@hospital.com"
            };

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, response);
        }
    }

    public class DoctorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CreateDoctorRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}