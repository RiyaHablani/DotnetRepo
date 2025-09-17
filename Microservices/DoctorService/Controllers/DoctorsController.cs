using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private static readonly List<Doctor> _doctors = new()
        {
            new Doctor { Id = 1, Name = "Dr. James Anderson", Specialization = "Cardiology", Email = "dr.anderson@hospital.com" },
            new Doctor { Id = 2, Name = "Dr. Lisa Martinez", Specialization = "Pediatrics", Email = "dr.martinez@hospital.com" },
            new Doctor { Id = 3, Name = "Dr. David Thompson", Specialization = "Orthopedics", Email = "dr.thompson@hospital.com" }
        };

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            return Ok(_doctors);
        }

        [HttpGet("{id}")]
        public IActionResult GetDoctor(int id)
        {
            var doctor = _doctors.FirstOrDefault(d => d.Id == id);
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult CreateDoctor([FromBody] Doctor doctor)
        {
            doctor.Id = _doctors.Count + 1;
            _doctors.Add(doctor);
            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
        }
    }

    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
