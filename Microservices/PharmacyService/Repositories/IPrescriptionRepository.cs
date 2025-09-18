using PharmacyService.Models.Entities;

namespace PharmacyService.Repositories
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(int patientId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByStatusAsync(string status);
        Task<Prescription?> GetPrescriptionWithMedicinesAsync(int id);
        Task<IEnumerable<Prescription>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}


