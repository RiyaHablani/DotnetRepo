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
    }
}
