using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillingService.Models.DTOs;
using BillingService.Services;

namespace BillingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication
    public class ExpendituresController : ControllerBase
    {
        private readonly IExpenditureService _expenditureService;

        public ExpendituresController(IExpenditureService expenditureService)
        {
            _expenditureService = expenditureService;
        }

        // GET: api/expenditures
        [HttpGet]
        public async Task<IActionResult> GetAllExpenditures()
        {
            var expenditures = await _expenditureService.GetAllExpendituresAsync();
            return Ok(expenditures);
        }

        // GET: api/expenditures/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenditure(int id)
        {
            var expenditure = await _expenditureService.GetExpenditureByIdAsync(id);
            if (expenditure == null)
                return NotFound();

            return Ok(expenditure);
        }

        // POST: api/expenditures
        [HttpPost]
        [Authorize(Roles = "Admin,Finance")] // Only admins and finance staff can create expenditures
        public async Task<IActionResult> CreateExpenditure([FromBody] CreateExpenditureDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var expenditure = await _expenditureService.CreateExpenditureAsync(createDto);
                return CreatedAtAction(nameof(GetExpenditure), new { id = expenditure.Id }, expenditure);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/expenditures/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Finance")] // Only admins and finance staff can update expenditures
        public async Task<IActionResult> UpdateExpenditure(int id, [FromBody] UpdateExpenditureDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var expenditure = await _expenditureService.UpdateExpenditureAsync(id, updateDto);
                if (expenditure == null)
                    return NotFound();

                return Ok(expenditure);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/expenditures/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only admins can delete expenditures
        public async Task<IActionResult> DeleteExpenditure(int id)
        {
            var result = await _expenditureService.DeleteExpenditureAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/expenditures/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchExpenditures([FromQuery] ExpenditureSearchDto searchDto)
        {
            var expenditures = await _expenditureService.SearchExpendituresAsync(searchDto);
            return Ok(expenditures);
        }
    }
}


