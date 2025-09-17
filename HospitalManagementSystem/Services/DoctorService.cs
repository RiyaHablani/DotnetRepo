using AutoMapper;
using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;
using HospitalManagementSystem.Repositories;

namespace HospitalManagementSystem.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<Doctor> doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto)
        {
            var doctor = _mapper.Map<Doctor>(doctorDto);
            var createdDoctor = await _doctorRepository.AddAsync(doctor);
            return _mapper.Map<DoctorDto>(createdDoctor);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorDto doctorDto)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);
            if (existingDoctor == null)
                return null;

            _mapper.Map(doctorDto, existingDoctor);
            await _doctorRepository.UpdateAsync(existingDoctor);
            return _mapper.Map<DoctorDto>(existingDoctor);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var exists = await _doctorRepository.ExistsAsync(id);
            if (!exists)
                return false;

            await _doctorRepository.DeleteAsync(id);
            return true;
        }

    }
}
