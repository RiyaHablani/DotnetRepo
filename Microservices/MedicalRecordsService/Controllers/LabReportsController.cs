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
    public class LabReportsController : ControllerBase
    {
        private readonly MedicalRecordsDbContext _context;

        public LabReportsController(MedicalRecordsDbContext context)
        {
            _context = context;
        }

        // GET: api/labreports/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetPatientLabReports(int patientId)
        {
            var userRole = GetUserRole();
            
            // Patients can only view their own lab reports
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != patientId)
                {
                    return Forbid("Patients can only view their own lab reports.");
                }
            }

            var reports = await _context.LabReports
                .Where(r => r.PatientId == patientId && !r.IsDeleted)
                .OrderByDescending(r => r.TestDate)
                .ToListAsync();

            return Ok(reports);
        }

        // GET: api/labreports/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLabReport(int id)
        {
            var report = await _context.LabReports
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (report == null)
                return NotFound();

            var userRole = GetUserRole();
            
            // Patients can only view their own lab reports
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != report.PatientId)
                {
                    return Forbid("Patients can only view their own lab reports.");
                }
            }

            return Ok(report);
        }

        // POST: api/labreports
        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can create lab reports
        public async Task<IActionResult> CreateLabReport([FromBody] LabReport report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            report.CreatedAt = DateTime.UtcNow;
            report.UpdatedAt = DateTime.UtcNow;

            _context.LabReports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLabReport), new { id = report.Id }, report);
        }

        // PUT: api/labreports/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can update lab reports
        public async Task<IActionResult> UpdateLabReport(int id, [FromBody] LabReport report)
        {
            if (id != report.Id)
                return BadRequest();

            var existingReport = await _context.LabReports
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (existingReport == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingReport.TestName = report.TestName;
            existingReport.TestDescription = report.TestDescription;
            existingReport.Results = report.Results;
            existingReport.Status = report.Status;
            existingReport.Notes = report.Notes;
            existingReport.TestDate = report.TestDate;
            existingReport.CompletedDate = report.CompletedDate;
            existingReport.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/labreports/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can delete lab reports
        public async Task<IActionResult> DeleteLabReport(int id)
        {
            var report = await _context.LabReports
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (report == null)
                return NotFound();

            report.IsDeleted = true;
            report.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/labreports/status/{status}
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Doctor,Admin")] // Only doctors and admins can filter by status
        public async Task<IActionResult> GetLabReportsByStatus(string status)
        {
            var reports = await _context.LabReports
                .Where(r => r.Status == status && !r.IsDeleted)
                .OrderByDescending(r => r.TestDate)
                .ToListAsync();

            return Ok(reports);
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
