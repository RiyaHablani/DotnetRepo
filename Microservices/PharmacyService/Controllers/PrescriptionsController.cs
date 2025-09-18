using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmacyService.Models.DTOs;
using PharmacyService.Services;
using System.Security.Claims;

namespace PharmacyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // GET: api/prescriptions
        [HttpGet]
        public async Task<IActionResult> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }

        // GET: api/prescriptions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrescription(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
                return NotFound();

            return Ok(prescription);
        }

        // POST: api/prescriptions
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can create prescriptions
        public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var prescription = await _prescriptionService.CreatePrescriptionAsync(createDto);
                return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, prescription);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/prescriptions/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Pharmacist,Admin")] // Only pharmacists and admins can update prescription status
        public async Task<IActionResult> UpdatePrescriptionStatus(int id, [FromBody] UpdatePrescriptionStatusDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var prescription = await _prescriptionService.UpdatePrescriptionStatusAsync(id, updateDto);
                if (prescription == null)
                    return NotFound();

                return Ok(prescription);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/prescriptions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can delete prescriptions
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var result = await _prescriptionService.DeletePrescriptionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/prescriptions/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPrescriptionsByPatientId(int patientId)
        {
            var userRole = GetUserRole();
            
            // Patients can only view their own prescriptions
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != patientId)
                {
                    return Forbid();
                }
            }

            var prescriptions = await _prescriptionService.GetPrescriptionsByPatientIdAsync(patientId);
            return Ok(prescriptions);
        }

        // GET: api/prescriptions/doctor/{doctorId}
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetPrescriptionsByDoctorId(int doctorId)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByDoctorIdAsync(doctorId);
            return Ok(prescriptions);
        }

        // GET: api/prescriptions/status/{status}
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPrescriptionsByStatus(string status)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByStatusAsync(status);
            return Ok(prescriptions);
        }

        // GET: api/prescriptions/date-range
        [HttpGet("date-range")]
        public async Task<IActionResult> GetPrescriptionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByDateRangeAsync(startDate, endDate);
            return Ok(prescriptions);
        }

        // POST: api/prescriptions/{prescriptionId}/medicines
        [HttpPost("{prescriptionId}/medicines")]
        [Authorize(Roles = "Pharmacist,Admin")] // Only pharmacists and admins can save patient medicines
        public async Task<IActionResult> SavePatientMedicine(int prescriptionId, [FromBody] CreatePatientMedicineDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var prescription = await _prescriptionService.SavePatientMedicineAsync(prescriptionId, createDto);
                if (prescription == null)
                    return NotFound();

                return Ok(prescription);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Helper methods
        private string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Patient";
        }

        private int GetPatientIdFromToken()
        {
            var patientIdClaim = User.FindFirst("PatientId")?.Value;
            return int.TryParse(patientIdClaim, out var patientId) ? patientId : 0;
        }
    }
}
