using AutoMapper;
using PharmacyService.Models.DTOs;
using PharmacyService.Models.Entities;
using PharmacyService.Repositories;

namespace PharmacyService.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;

        public MedicineService(IMedicineRepository medicineRepository, IMapper mapper)
        {
            _medicineRepository = medicineRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync()
        {
            var medicines = await _medicineRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
        }

        public async Task<MedicineDto?> GetMedicineByIdAsync(int id)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            return medicine == null ? null : _mapper.Map<MedicineDto>(medicine);
        }

        public async Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto createDto)
        {
            // Check if medicine name is unique
            if (!await _medicineRepository.IsMedicineNameUniqueAsync(createDto.Name))
            {
                throw new InvalidOperationException("A medicine with this name already exists.");
            }

            var medicine = _mapper.Map<Medicine>(createDto);
            medicine.CreatedAt = DateTime.UtcNow;
            medicine.UpdatedAt = DateTime.UtcNow;

            var createdMedicine = await _medicineRepository.AddAsync(medicine);
            return _mapper.Map<MedicineDto>(createdMedicine);
        }

        public async Task<MedicineDto?> UpdateMedicineAsync(int id, UpdateMedicineDto updateDto)
        {
            var existingMedicine = await _medicineRepository.GetByIdAsync(id);
            if (existingMedicine == null)
                return null;

            // Check if medicine name is unique (excluding current medicine)
            if (!await _medicineRepository.IsMedicineNameUniqueAsync(updateDto.Name, id))
            {
                throw new InvalidOperationException("A medicine with this name already exists.");
            }

            _mapper.Map(updateDto, existingMedicine);
            existingMedicine.UpdatedAt = DateTime.UtcNow;

            await _medicineRepository.UpdateAsync(existingMedicine);
            return _mapper.Map<MedicineDto>(existingMedicine);
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            var medicine = await _medicineRepository.GetByIdAsync(id);
            if (medicine == null)
                return false;

            await _medicineRepository.DeleteAsync(medicine);
            return true;
        }

        public async Task<IEnumerable<MedicineDto>> GetUnexpiredMedicinesAsync()
        {
            var medicines = await _medicineRepository.GetUnexpiredMedicinesAsync();
            return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
        }

        public async Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(MedicineSearchDto searchDto)
        {
            var medicines = await _medicineRepository.SearchMedicinesAsync(searchDto);
            return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
        }

        public async Task<IEnumerable<MedicineDto>> GetMedicinesByDiseaseTypeAsync(string diseaseType)
        {
            var medicines = await _medicineRepository.GetMedicinesByDiseaseTypeAsync(diseaseType);
            return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
        }

        public async Task<IEnumerable<MedicineDto>> GetExpiringMedicinesAsync(int daysFromNow)
        {
            var medicines = await _medicineRepository.GetExpiringMedicinesAsync(daysFromNow);
            return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
        }

        public async Task<bool> IsMedicineNameUniqueAsync(string name, int? excludeId = null)
        {
            return await _medicineRepository.IsMedicineNameUniqueAsync(name, excludeId);
        }
    }
}
