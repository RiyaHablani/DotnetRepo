using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppointmentService.Models.DTOs;
using AppointmentService.Services;

namespace AppointmentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving appointments", error = ex.Message });
            }
        }

        /// <summary>
        /// Get appointment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                    return NotFound(new { message = "Appointment not found" });
                
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the appointment", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist,Patient")] // Multiple roles can create appointments
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _appointmentService.CreateAppointmentAsync(createDto);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the appointment", error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing appointment
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can update appointments
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appointment = await _appointmentService.UpdateAppointmentAsync(id, updateDto);
                if (appointment == null)
                    return NotFound(new { message = "Appointment not found" });

                return Ok(appointment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the appointment", error = ex.Message });
            }
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin,Receptionist,Patient")] // Multiple roles can cancel appointments
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                var result = await _appointmentService.CancelAppointmentAsync(id);
                if (!result)
                    return NotFound(new { message = "Appointment not found" });

                return Ok(new { message = "Appointment cancelled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while cancelling the appointment", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an appointment (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete appointments
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var result = await _appointmentService.DeleteAppointmentAsync(id);
                if (!result)
                    return NotFound(new { message = "Appointment not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the appointment", error = ex.Message });
            }
        }

        /// <summary>
        /// Get appointments by doctor ID
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId, [FromQuery] DateTime? date = null)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, date);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving doctor appointments", error = ex.Message });
            }
        }

        /// <summary>
        /// Get appointments by patient ID
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByPatientAsync(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving patient appointments", error = ex.Message });
            }
        }

        /// <summary>
        /// Get available time slots for a doctor on a specific date
        /// </summary>
        [HttpGet("doctor/{doctorId}/available-slots")]
        public async Task<IActionResult> GetAvailableTimeSlots(int doctorId, [FromQuery] DateTime date, [FromQuery] int duration = 30)
        {
            try
            {
                var timeSlots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date, duration);
                return Ok(new { doctorId, date, duration, availableSlots = timeSlots });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving available time slots", error = ex.Message });
            }
        }

        /// <summary>
        /// Test endpoint to verify service is working
        /// </summary>
        [HttpGet("test")]
        [AllowAnonymous] // Allow access without authentication
        public IActionResult Test()
        {
            return Ok(new { message = "Appointment service is working", timestamp = DateTime.UtcNow });
        }
    }
}
