using AutoMapper;
using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient mappings
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();

            // Doctor mappings
            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>();
        }
    }
}
