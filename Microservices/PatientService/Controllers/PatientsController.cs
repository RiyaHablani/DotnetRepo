using Microsoft.AspNetCore.Mvc;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private static readonly List<Patient> _patients = new()
        {
            new Patient { Id = 1, Name = "John Smith", Age = 35, Gender = "Male", Address = "123 Main St, New York, NY" },
            new Patient { Id = 2, Name = "Sarah Johnson", Age = 28, Gender = "Female", Address = "456 Oak Ave, Los Angeles, CA" },
            new Patient { Id = 3, Name = "Michael Brown", Age = 42, Gender = "Male", Address = "789 Pine Rd, Chicago, IL" }
        };

        [HttpGet]
        public IActionResult GetAllPatients()
        {
            return Ok(_patients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            var patient = _patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            patient.Id = _patients.Count + 1;
            _patients.Add(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
