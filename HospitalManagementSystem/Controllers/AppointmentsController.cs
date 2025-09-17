using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentService appointmentService, ILogger<AppointmentsController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all appointments with optional filtering
        /// </summary>
        /// <returns>List of appointments</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all appointments");
                return StatusCode(500, new { message = "An error occurred while retrieving appointments" });
            }
        }

        /// <summary>
        /// Get appointment by ID
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>Appointment details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointmentById(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { message = "Appointment not found" });
                }

                // Authorization: Users can only view their own appointments unless they're admin/doctor
                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var userId = HttpContext.Items["UserId"];
                var patientId = HttpContext.Items["PatientId"];
                var doctorId = HttpContext.Items["DoctorId"];

                if (userRole != "Admin" && userRole != "Doctor")
                {
                    if (patientId == null || appointment.PatientId != (int)patientId)
                    {
                        return Forbid();
                    }
                }

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the appointment" });
            }
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        /// <param name="createAppointmentDto">Appointment creation data</param>
        /// <returns>Created appointment</returns>
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentDto createAppointmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Authorization: Patients can only book appointments for themselves
                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var patientId = HttpContext.Items["PatientId"];

                if (userRole == "Patient" && (patientId == null || createAppointmentDto.PatientId != (int)patientId))
                {
                    return Forbid();
                }

                var appointment = await _appointmentService.CreateAppointmentAsync(createAppointmentDto);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment");
                return StatusCode(500, new { message = "An error occurred while creating the appointment" });
            }
        }

        /// <summary>
        /// Update an existing appointment
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <param name="updateAppointmentDto">Appointment update data</param>
        /// <returns>Updated appointment</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if appointment exists and user has permission
                var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (existingAppointment == null)
                {
                    return NotFound(new { message = "Appointment not found" });
                }

                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var patientId = HttpContext.Items["PatientId"];
                var doctorId = HttpContext.Items["DoctorId"];

                // Authorization check
                if (userRole != "Admin")
                {
                    if (userRole == "Patient" && (patientId == null || existingAppointment.PatientId != (int)patientId))
                    {
                        return Forbid();
                    }
                    if (userRole == "Doctor" && (doctorId == null || existingAppointment.DoctorId != (int)doctorId))
                    {
                        return Forbid();
                    }
                }

                var appointment = await _appointmentService.UpdateAppointmentAsync(id, updateAppointmentDto);
                return Ok(appointment);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment with ID: {AppointmentId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the appointment" });
            }
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        /// <param name="id">Appointment ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            try
            {
                // Check if appointment exists and user has permission
                var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (existingAppointment == null)
                {
                    return NotFound(new { message = "Appointment not found" });
                }

                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var patientId = HttpContext.Items["PatientId"];
                var doctorId = HttpContext.Items["DoctorId"];

                // Authorization check
                if (userRole != "Admin")
                {
                    if (userRole == "Patient" && (patientId == null || existingAppointment.PatientId != (int)patientId))
                    {
                        return Forbid();
                    }
                    if (userRole == "Doctor" && (doctorId == null || existingAppointment.DoctorId != (int)doctorId))
                    {
                        return Forbid();
                    }
                }

                var success = await _appointmentService.CancelAppointmentAsync(id);
                if (!success)
                {
                    return NotFound(new { message = "Appointment not found" });
                }

                return Ok(new { message = "Appointment cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling appointment with ID: {AppointmentId}", id);
                return StatusCode(500, new { message = "An error occurred while cancelling the appointment" });
            }
        }

        /// <summary>
        /// Get appointments by doctor ID
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <param name="date">Optional date filter</param>
        /// <returns>List of appointments for the doctor</returns>
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctor(int doctorId, [FromQuery] DateTime? date = null)
        {
            try
            {
                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var userDoctorId = HttpContext.Items["DoctorId"];

                // Authorization: Doctors can only view their own appointments
                if (userRole == "Doctor" && (userDoctorId == null || doctorId != (int)userDoctorId))
                {
                    return Forbid();
                }

                var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, date);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for doctor: {DoctorId}", doctorId);
                return StatusCode(500, new { message = "An error occurred while retrieving appointments" });
            }
        }

        /// <summary>
        /// Get appointments by patient ID
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <returns>List of appointments for the patient</returns>
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByPatient(int patientId)
        {
            try
            {
                var userRole = HttpContext.Items["UserRole"]?.ToString();
                var userPatientId = HttpContext.Items["PatientId"];

                // Authorization: Patients can only view their own appointments
                if (userRole == "Patient" && (userPatientId == null || patientId != (int)userPatientId))
                {
                    return Forbid();
                }

                var appointments = await _appointmentService.GetAppointmentsByPatientAsync(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for patient: {PatientId}", patientId);
                return StatusCode(500, new { message = "An error occurred while retrieving appointments" });
            }
        }

        /// <summary>
        /// Get available time slots for a doctor on a specific date
        /// </summary>
        /// <param name="doctorId">Doctor ID</param>
        /// <param name="date">Date to check availability</param>
        /// <param name="duration">Appointment duration in minutes (default: 30)</param>
        /// <returns>List of available time slots</returns>
        [HttpGet("available-slots")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableTimeSlots([FromQuery] int doctorId, [FromQuery] DateTime date, [FromQuery] int duration = 30)
        {
            try
            {
                if (doctorId <= 0)
                {
                    return BadRequest(new { message = "Doctor ID is required" });
                }

                if (date.Date < DateTime.Today)
                {
                    return BadRequest(new { message = "Cannot check availability for past dates" });
                }

                var availableSlots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date, duration);
                return Ok(availableSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available time slots for doctor: {DoctorId} on {Date}", doctorId, date);
                return StatusCode(500, new { message = "An error occurred while retrieving available time slots" });
            }
        }
    }
}
