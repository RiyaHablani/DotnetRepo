using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillingService.Models.DTOs;
using BillingService.Services;
using System.Security.Claims;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        // GET: api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        // POST: api/invoices
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can create invoices
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(createDto);
                return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/invoices/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can update invoices
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var invoice = await _invoiceService.UpdateInvoiceAsync(id, updateDto);
                if (invoice == null)
                    return NotFound();

                return Ok(invoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/invoices/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete invoices
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // POST: api/invoices/{id}/mark-paid
        [HttpPost("{id}/mark-paid")]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can mark invoices as paid
        public async Task<IActionResult> MarkInvoicePaid(int id, [FromBody] MarkInvoicePaidDto markPaidDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var invoice = await _invoiceService.MarkInvoicePaidAsync(id, markPaidDto);
                if (invoice == null)
                    return NotFound();

                return Ok(invoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/invoices/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetInvoicesByPatientId(int patientId)
        {
            var userRole = GetUserRole();
            
            // Patients can only view their own invoices
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != patientId)
                {
                    return Forbid();
                }
            }

            var invoices = await _invoiceService.GetInvoicesByPatientIdAsync(patientId);
            return Ok(invoices);
        }

        // GET: api/invoices/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchInvoices([FromQuery] InvoiceSearchDto searchDto)
        {
            var invoices = await _invoiceService.SearchInvoicesAsync(searchDto);
            return Ok(invoices);
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


