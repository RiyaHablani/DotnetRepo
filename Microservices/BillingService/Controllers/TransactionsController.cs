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
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        // POST: api/transactions
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can create transactions
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(createDto);
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/transactions/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist")] // Only admins and receptionists can update transactions
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transaction = await _transactionService.UpdateTransactionAsync(id, updateDto);
                if (transaction == null)
                    return NotFound();

                return Ok(transaction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/transactions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete transactions
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/transactions/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetTransactionsByPatientId(int patientId)
        {
            var userRole = GetUserRole();
            
            // Patients can only view their own transactions
            if (userRole == "Patient")
            {
                var patientIdFromToken = GetPatientIdFromToken();
                if (patientIdFromToken != patientId)
                {
                    return Forbid();
                }
            }

            var transactions = await _transactionService.GetTransactionsByPatientIdAsync(patientId);
            return Ok(transactions);
        }

        // GET: api/transactions/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchTransactions([FromQuery] TransactionSearchDto searchDto)
        {
            var transactions = await _transactionService.SearchTransactionsAsync(searchDto);
            return Ok(transactions);
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


