namespace PharmacyService.Models.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DiseaseType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Manufacturer { get; set; }
        public string? DosageForm { get; set; }
        public string? DosageStrength { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;
    }
}
