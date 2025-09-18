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
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        // GET: api/medicines
        [HttpGet]
        public async Task<IActionResult> GetAllMedicines()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        // GET: api/medicines/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicine(int id)
        {
            var medicine = await _medicineService.GetMedicineByIdAsync(id);
            if (medicine == null)
                return NotFound();

            return Ok(medicine);
        }

        // POST: api/medicines
        [HttpPost]
        [Authorize(Roles = "Admin,Pharmacist")] // Only admins and pharmacists can create medicines
        public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var medicine = await _medicineService.CreateMedicineAsync(createDto);
                return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, medicine);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/medicines/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Pharmacist")] // Only admins and pharmacists can update medicines
        public async Task<IActionResult> UpdateMedicine(int id, [FromBody] UpdateMedicineDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var medicine = await _medicineService.UpdateMedicineAsync(id, updateDto);
                if (medicine == null)
                    return NotFound();

                return Ok(medicine);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/medicines/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Pharmacist")] // Only admins and pharmacists can delete medicines
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var result = await _medicineService.DeleteMedicineAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/medicines/unexpired
        [HttpGet("unexpired")]
        public async Task<IActionResult> GetUnexpiredMedicines()
        {
            var medicines = await _medicineService.GetUnexpiredMedicinesAsync();
            return Ok(medicines);
        }

        // GET: api/medicines/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchMedicines([FromQuery] MedicineSearchDto searchDto)
        {
            var medicines = await _medicineService.SearchMedicinesAsync(searchDto);
            return Ok(medicines);
        }

        // GET: api/medicines/disease-type/{diseaseType}
        [HttpGet("disease-type/{diseaseType}")]
        public async Task<IActionResult> GetMedicinesByDiseaseType(string diseaseType)
        {
            var medicines = await _medicineService.GetMedicinesByDiseaseTypeAsync(diseaseType);
            return Ok(medicines);
        }

        // GET: api/medicines/expiring/{daysFromNow}
        [HttpGet("expiring/{daysFromNow}")]
        [Authorize(Roles = "Admin,Pharmacist")] // Only admins and pharmacists can view expiring medicines
        public async Task<IActionResult> GetExpiringMedicines(int daysFromNow)
        {
            var medicines = await _medicineService.GetExpiringMedicinesAsync(daysFromNow);
            return Ok(medicines);
        }

        // GET: api/medicines/check-name-unique
        [HttpGet("check-name-unique")]
        [Authorize(Roles = "Admin,Pharmacist")] // Only admins and pharmacists can check name uniqueness
        public async Task<IActionResult> CheckNameUnique([FromQuery] string name, [FromQuery] int? excludeId = null)
        {
            var isUnique = await _medicineService.IsMedicineNameUniqueAsync(name, excludeId);
            return Ok(new { isUnique });
        }
    }
}


