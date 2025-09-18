using AppointmentService.Models.DTOs;

namespace AppointmentService.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
        Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto createDto);
        Task<AppointmentDto?> UpdateAppointmentAsync(int id, UpdateAppointmentDto updateDto);
        Task<bool> DeleteAppointmentAsync(int id);
        Task<bool> CancelAppointmentAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(int patientId);
        Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, int duration = 30);
        Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate, int duration, int? excludeAppointmentId = null);
    }
}


