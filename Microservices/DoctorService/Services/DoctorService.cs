using AutoMapper;
using DoctorService.Models.DTOs;
using DoctorService.Models.Entities;
using DoctorService.Repositories;

namespace DoctorService.Services
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

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            var doctor = _mapper.Map<Doctor>(createDoctorDto);
            doctor.CreatedAt = DateTime.UtcNow;
            doctor.UpdatedAt = DateTime.UtcNow;
            doctor.IsActive = true;

            var createdDoctor = await _doctorRepository.AddAsync(doctor);
            return _mapper.Map<DoctorDto>(createdDoctor);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);
            if (existingDoctor == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateDoctorDto.Name))
                existingDoctor.Name = updateDoctorDto.Name;
            
            if (!string.IsNullOrEmpty(updateDoctorDto.Specialization))
                existingDoctor.Specialization = updateDoctorDto.Specialization;
            
            if (!string.IsNullOrEmpty(updateDoctorDto.Email))
                existingDoctor.Email = updateDoctorDto.Email;
            
            if (updateDoctorDto.IsActive.HasValue)
                existingDoctor.IsActive = updateDoctorDto.IsActive.Value;

            existingDoctor.UpdatedAt = DateTime.UtcNow;

            var updatedDoctor = await _doctorRepository.UpdateAsync(existingDoctor);
            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            return await _doctorRepository.DeleteAsync(id);
        }

        public async Task<bool> DoctorExistsAsync(int id)
        {
            return await _doctorRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(string specialization)
        {
            var doctors = await _doctorRepository.FindAsync(d => 
                d.Specialization.ToLower().Contains(specialization.ToLower()) && d.IsActive);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
    }
}
