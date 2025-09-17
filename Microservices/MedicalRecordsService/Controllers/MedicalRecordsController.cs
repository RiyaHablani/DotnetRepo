using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MedicalRecordsService.Models.Entities;
using MedicalRecordsService.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalRecordsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class MedicalRecordsController : ControllerBase
    {
        private readonly MedicalRecordsDbContext _context;

        public MedicalRecordsController(MedicalRecordsDbContext context)
        {
            _context = context;
        }

        // GET: api/medicalrecords/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientRecords(int patientId)
        {
            var userRole = GetUserRole();
            
            // Patients can only view their own records
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != patientId)
                {
                    return Forbid("Patients can only view their own medical records.");
                }
            }

            var records = await _context.MedicalRecords
                .Where(r => r.PatientId == patientId && !r.IsDeleted)
                .OrderByDescending(r => r.RecordDate)
                .ToListAsync();

            return Ok(records);
        }

        // GET: api/medicalrecords/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecord(int id)
        {
            var record = await _context.MedicalRecords
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (record == null)
                return NotFound();

            var userRole = GetUserRole();
            
            // Patients can only view their own records
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != record.PatientId)
                {
                    return Forbid("Patients can only view their own medical records.");
                }
            }

            return Ok(record);
        }

        // POST: api/medicalrecords
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can create records
        public async Task<IActionResult> CreateRecord([FromBody] MedicalRecord record)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            record.CreatedAt = DateTime.UtcNow;
            record.UpdatedAt = DateTime.UtcNow;

            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecord), new { id = record.Id }, record);
        }

        // PUT: api/medicalrecords/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can update records
        public async Task<IActionResult> UpdateRecord(int id, [FromBody] MedicalRecord record)
        {
            if (id != record.Id)
                return BadRequest();

            var existingRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (existingRecord == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingRecord.Diagnosis = record.Diagnosis;
            existingRecord.Symptoms = record.Symptoms;
            existingRecord.Treatment = record.Treatment;
            existingRecord.Notes = record.Notes;
            existingRecord.RecordDate = record.RecordDate;
            existingRecord.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/medicalrecords/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can delete records
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var record = await _context.MedicalRecords
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (record == null)
                return NotFound();

            record.IsDeleted = true;
            record.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
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
