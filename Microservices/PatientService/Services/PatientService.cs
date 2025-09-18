using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Models.DTOs;
using PatientService.Models.Entities;
using PatientService.Repositories;

namespace PatientService.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;
        private readonly HospitalDbContext _context;

        public PatientService(
            IRepository<Patient> patientRepository,
            IMapper mapper,
            ILogger<PatientService> logger,
            HospitalDbContext context)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _context.Patients
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<PatientDto>>(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

                return patient == null ? null : _mapper.Map<PatientDto>(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto)
        {
            try
            {
                var patient = _mapper.Map<Patient>(createDto);
                patient.CreatedAt = DateTime.UtcNow;
                patient.UpdatedAt = DateTime.UtcNow;

                var createdPatient = await _patientRepository.AddAsync(patient);

                _logger.LogInformation("Patient created successfully. ID: {PatientId}, Name: {Name}", 
                    createdPatient.Id, createDto.Name);

                return _mapper.Map<PatientDto>(createdPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient with Name: {Name}", createDto.Name);
                throw;
            }
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updateDto)
        {
            try
            {
                var existingPatient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

                if (existingPatient == null)
                    return null;

                _mapper.Map(updateDto, existingPatient);
                existingPatient.UpdatedAt = DateTime.UtcNow;

                await _patientRepository.UpdateAsync(existingPatient);

                _logger.LogInformation("Patient updated successfully. ID: {PatientId}", id);

                return _mapper.Map<PatientDto>(existingPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID: {PatientId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);
                if (patient == null || patient.IsDeleted)
                    return false;

                patient.IsDeleted = true;
                patient.UpdatedAt = DateTime.UtcNow;

                await _patientRepository.UpdateAsync(patient);

                _logger.LogInformation("Patient deleted successfully. ID: {PatientId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", id);
                throw;
            }
        }
    }
}


