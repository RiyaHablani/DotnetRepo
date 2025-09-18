using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class MedicineSearchDto
    {
        public string? Name { get; set; }
        public string? DiseaseType { get; set; }
        public string? Manufacturer { get; set; }
        public string? DosageForm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsUnexpired { get; set; }
        public DateTime? ExpiryDateFrom { get; set; }
        public DateTime? ExpiryDateTo { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public bool? IncludeExpired { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
