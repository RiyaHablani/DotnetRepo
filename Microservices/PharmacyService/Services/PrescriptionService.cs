using AutoMapper;
using PharmacyService.Models.DTOs;
using PharmacyService.Models.Entities;
using PharmacyService.Repositories;

namespace PharmacyService.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPatientMedicineRepository _patientMedicineRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository,
            IPatientMedicineRepository patientMedicineRepository,
            IMedicineRepository medicineRepository,
            IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _patientMedicineRepository = patientMedicineRepository;
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
        {
            var prescriptions = await _prescriptionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id)
        {
            var prescription = await _prescriptionRepository.GetPrescriptionWithMedicinesAsync(id);
            return prescription == null ? null : _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto)
        {
            var prescription = _mapper.Map<Prescription>(createDto);
            prescription.CreatedAt = DateTime.UtcNow;
            prescription.UpdatedAt = DateTime.UtcNow;

            var createdPrescription = await _prescriptionRepository.AddAsync(prescription);

            // Add patient medicines
            foreach (var patientMedicineDto in createDto.PatientMedicines)
            {
                var patientMedicine = _mapper.Map<PatientMedicine>(patientMedicineDto);
                patientMedicine.PrescriptionId = createdPrescription.Id;
                patientMedicine.CreatedAt = DateTime.UtcNow;
                patientMedicine.UpdatedAt = DateTime.UtcNow;

                await _patientMedicineRepository.AddAsync(patientMedicine);
            }

            // Reload prescription with medicines
            var prescriptionWithMedicines = await _prescriptionRepository.GetPrescriptionWithMedicinesAsync(createdPrescription.Id);
            return _mapper.Map<PrescriptionDto>(prescriptionWithMedicines);
        }

        public async Task<PrescriptionDto?> UpdatePrescriptionStatusAsync(int id, UpdatePrescriptionStatusDto updateDto)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
                return null;

            prescription.Status = updateDto.Status;
            prescription.FilledBy = updateDto.FilledBy;
            prescription.UpdatedAt = DateTime.UtcNow;

            if (updateDto.Status == "Filled")
            {
                prescription.FilledDate = DateTime.UtcNow;
            }

            await _prescriptionRepository.UpdateAsync(prescription);

            // Reload prescription with medicines
            var prescriptionWithMedicines = await _prescriptionRepository.GetPrescriptionWithMedicinesAsync(id);
            return _mapper.Map<PrescriptionDto>(prescriptionWithMedicines);
        }

        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);
            if (prescription == null)
                return false;

            await _prescriptionRepository.DeleteAsync(prescription);
            return true;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int patientId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByPatientIdAsync(patientId);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByDoctorIdAsync(int doctorId)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByDoctorIdAsync(doctorId);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByStatusAsync(string status)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByStatusAsync(status);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var prescriptions = await _prescriptionRepository.GetPrescriptionsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }

        public async Task<PrescriptionDto?> SavePatientMedicineAsync(int prescriptionId, CreatePatientMedicineDto createDto)
        {
            // Verify prescription exists
            var prescription = await _prescriptionRepository.GetByIdAsync(prescriptionId);
            if (prescription == null)
                return null;

            // Verify medicine exists
            var medicine = await _medicineRepository.GetByIdAsync(createDto.MedicineId);
            if (medicine == null)
                throw new InvalidOperationException("Medicine not found.");

            // Check if medicine is available
            if (medicine.Quantity < createDto.Quantity)
                throw new InvalidOperationException("Insufficient medicine quantity available.");

            // Create patient medicine
            var patientMedicine = _mapper.Map<PatientMedicine>(createDto);
            patientMedicine.PrescriptionId = prescriptionId;
            patientMedicine.CreatedAt = DateTime.UtcNow;
            patientMedicine.UpdatedAt = DateTime.UtcNow;

            await _patientMedicineRepository.AddAsync(patientMedicine);

            // Update medicine quantity
            medicine.Quantity -= createDto.Quantity;
            medicine.UpdatedAt = DateTime.UtcNow;
            await _medicineRepository.UpdateAsync(medicine);

            // Reload prescription with medicines
            var prescriptionWithMedicines = await _prescriptionRepository.GetPrescriptionWithMedicinesAsync(prescriptionId);
            return _mapper.Map<PrescriptionDto>(prescriptionWithMedicines);
        }
    }
}
