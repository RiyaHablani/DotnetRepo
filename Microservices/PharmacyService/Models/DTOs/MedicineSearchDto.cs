namespace PharmacyService.Models.DTOs
{
    public class MedicineSearchDto
    {
        public string? Name { get; set; }
        public string? DiseaseType { get; set; }
        public DateTime? ExpiryDateFrom { get; set; }
        public DateTime? ExpiryDateTo { get; set; }
        public bool? IncludeExpired { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
