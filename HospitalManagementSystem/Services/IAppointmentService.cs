using HospitalManagementSystem.Models.DTOs;
using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);
        Task<AppointmentDto?> UpdateAppointmentAsync(int id, UpdateAppointmentDto updateAppointmentDto);
        Task<bool> CancelAppointmentAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(int patientId);
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate, int duration, int? excludeAppointmentId = null);
        Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, int duration = 30);
    }
}
