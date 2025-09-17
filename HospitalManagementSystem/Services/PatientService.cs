using AutoMapper;
using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;
using HospitalManagementSystem.Repositories;

namespace HospitalManagementSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IRepository<Patient> patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto patientDto)
        {
            var patient = _mapper.Map<Patient>(patientDto);
            var createdPatient = await _patientRepository.AddAsync(patient);
            return _mapper.Map<PatientDto>(createdPatient);
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, PatientDto patientDto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null)
                return null;

            _mapper.Map(patientDto, existingPatient);
            await _patientRepository.UpdateAsync(existingPatient);
            return _mapper.Map<PatientDto>(existingPatient);
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var exists = await _patientRepository.ExistsAsync(id);
            if (!exists)
                return false;

            await _patientRepository.DeleteAsync(id);
            return true;
        }

    }
}
