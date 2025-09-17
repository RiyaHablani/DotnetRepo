using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> _appointments = new()
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentDate = DateTime.Today.AddDays(1), Duration = 30, Status = "Scheduled" },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentDate = DateTime.Today.AddDays(2), Duration = 45, Status = "Scheduled" }
        };

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            return Ok(_appointments);
        }

        [HttpGet("{id}")]
        public IActionResult GetAppointment(int id)
        {
            var appointment = _appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public IActionResult CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var appointment = new Appointment
            {
                Id = _appointments.Count + 1,
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                Duration = request.Duration,
                Status = "Scheduled"
            };
            _appointments.Add(appointment);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Duration { get; set; }
    }
}
