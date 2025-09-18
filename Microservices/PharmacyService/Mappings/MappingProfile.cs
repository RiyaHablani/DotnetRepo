using AutoMapper;
using PharmacyService.Models.Entities;
using PharmacyService.Models.DTOs;

namespace PharmacyService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Medicine mappings
            CreateMap<Medicine, MedicineDto>();
            CreateMap<CreateMedicineDto, Medicine>();
            CreateMap<UpdateMedicineDto, Medicine>();

            // Prescription mappings
            CreateMap<Prescription, PrescriptionDto>();
            CreateMap<CreatePrescriptionDto, Prescription>();
            CreateMap<UpdatePrescriptionStatusDto, Prescription>();

            // PatientMedicine mappings
            CreateMap<PatientMedicine, PatientMedicineDto>()
                .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name));
            CreateMap<CreatePatientMedicineDto, PatientMedicine>();
        }
    }
}


