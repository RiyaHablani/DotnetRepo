using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AppointmentService.Data;
using AppointmentService.Models.DTOs;
using AppointmentService.Models.Entities;
using AppointmentService.Models.Enums;
using AppointmentService.Repositories;

namespace AppointmentService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly DoctorServiceClient _doctorClient;
        private readonly PatientServiceClient _patientClient;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;
        private readonly HospitalDbContext _context;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            DoctorServiceClient doctorClient,
            PatientServiceClient patientClient,
            IMapper mapper,
            ILogger<AppointmentService> logger,
            HospitalDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _doctorClient = doctorClient;
            _patientClient = patientClient;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => !a.IsDeleted)
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();

                var appointmentDtos = new List<AppointmentDto>();
                
                foreach (var appointment in appointments)
                {
                    var patient = await _patientClient.GetPatientAsync(appointment.PatientId);
                    var doctor = await _doctorClient.GetDoctorAsync(appointment.DoctorId);
                    
                    appointmentDtos.Add(new AppointmentDto
                    {
                        Id = appointment.Id,
                        PatientId = appointment.PatientId,
                        PatientName = patient?.Name ?? "Unknown",
                        DoctorId = appointment.DoctorId,
                        DoctorName = doctor?.Name ?? "Unknown",
                        DoctorSpecialization = doctor?.Specialization ?? "Unknown",
                        AppointmentDate = appointment.AppointmentDate,
                        Duration = appointment.Duration,
                        Status = appointment.Status,
                        CreatedAt = appointment.CreatedAt
                    });
                }

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all appointments");
                throw;
            }
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

                if (appointment == null)
                    return null;

                // Fetch patient and doctor details
                var patient = await _patientClient.GetPatientAsync(appointment.PatientId);
                var doctor = await _doctorClient.GetDoctorAsync(appointment.DoctorId);

                return new AppointmentDto
                {
                    Id = appointment.Id,
                    PatientId = appointment.PatientId,
                    PatientName = patient?.Name ?? "Unknown",
                    DoctorId = appointment.DoctorId,
                    DoctorName = doctor?.Name ?? "Unknown",
                    DoctorSpecialization = doctor?.Specialization ?? "Unknown",
                    AppointmentDate = appointment.AppointmentDate,
                    Duration = appointment.Duration,
                    Status = appointment.Status,
                    CreatedAt = appointment.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto)
        {
            try
            {
                // Validate patient exists
                var patient = await _patientClient.GetPatientAsync(createAppointmentDto.PatientId);
                if (patient == null)
                {
                    throw new ArgumentException("Patient not found");
                }

                // Validate doctor exists
                var doctor = await _doctorClient.GetDoctorAsync(createAppointmentDto.DoctorId);
                if (doctor == null)
                {
                    throw new ArgumentException("Doctor not found or inactive");
                }

                // Check if doctor is available
                if (!await IsDoctorAvailableAsync(createAppointmentDto.DoctorId, createAppointmentDto.AppointmentDate, createAppointmentDto.Duration))
                {
                    throw new InvalidOperationException("Doctor is not available at the requested time");
                }

                // Create appointment
                var appointment = _mapper.Map<Appointment>(createAppointmentDto);
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.UpdatedAt = DateTime.UtcNow;

                var createdAppointment = await _appointmentRepository.AddAsync(appointment);

                // Load appointment for the response
                var appointmentWithIncludes = await _context.Appointments
                    .FirstAsync(a => a.Id == createdAppointment.Id);

                _logger.LogInformation("Appointment created successfully. ID: {AppointmentId}, Patient: {PatientId}, Doctor: {DoctorId}", 
                    createdAppointment.Id, createAppointmentDto.PatientId, createAppointmentDto.DoctorId);

                return _mapper.Map<AppointmentDto>(appointmentWithIncludes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating appointment for Patient: {PatientId}, Doctor: {DoctorId}", 
                    createAppointmentDto.PatientId, createAppointmentDto.DoctorId);
                throw;
            }
        }

        public async Task<AppointmentDto?> UpdateAppointmentAsync(int id, UpdateAppointmentDto updateAppointmentDto)
        {
            try
            {
                var existingAppointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

                if (existingAppointment == null)
                    return null;

                // If updating appointment date/time, check availability
                if (updateAppointmentDto.AppointmentDate.HasValue)
                {
                    var duration = updateAppointmentDto.Duration ?? existingAppointment.Duration;
                    if (!await IsDoctorAvailableAsync(existingAppointment.DoctorId, updateAppointmentDto.AppointmentDate.Value, duration, id))
                    {
                        throw new InvalidOperationException("Doctor is not available at the requested time");
                    }
                }

                // Apply updates
                _mapper.Map(updateAppointmentDto, existingAppointment);
                existingAppointment.UpdatedAt = DateTime.UtcNow;

                await _appointmentRepository.UpdateAsync(existingAppointment);

                _logger.LogInformation("Appointment updated successfully. ID: {AppointmentId}", id);

                return _mapper.Map<AppointmentDto>(existingAppointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<bool> CancelAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null || appointment.IsDeleted)
                    return false;

                appointment.Status = AppointmentStatus.Cancelled;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _appointmentRepository.UpdateAsync(appointment);

                _logger.LogInformation("Appointment cancelled successfully. ID: {AppointmentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null || appointment.IsDeleted)
                    return false;

                appointment.IsDeleted = true;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _appointmentRepository.UpdateAsync(appointment);

                _logger.LogInformation("Appointment deleted successfully. ID: {AppointmentId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting appointment with ID: {AppointmentId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null)
        {
            try
            {
                var query = _context.Appointments
                    .Where(a => a.DoctorId == doctorId && !a.IsDeleted);

                if (date.HasValue)
                {
                    var startDate = date.Value.Date;
                    var endDate = startDate.AddDays(1);
                    query = query.Where(a => a.AppointmentDate >= startDate && a.AppointmentDate < endDate);
                }

                var appointments = await query
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();

                var appointmentDtos = new List<AppointmentDto>();
                
                foreach (var appointment in appointments)
                {
                    var patient = await _patientClient.GetPatientAsync(appointment.PatientId);
                    var doctor = await _doctorClient.GetDoctorAsync(appointment.DoctorId);
                    
                    appointmentDtos.Add(new AppointmentDto
                    {
                        Id = appointment.Id,
                        PatientId = appointment.PatientId,
                        PatientName = patient?.Name ?? "Unknown",
                        DoctorId = appointment.DoctorId,
                        DoctorName = doctor?.Name ?? "Unknown",
                        DoctorSpecialization = doctor?.Specialization ?? "Unknown",
                        AppointmentDate = appointment.AppointmentDate,
                        Duration = appointment.Duration,
                        Status = appointment.Status,
                        CreatedAt = appointment.CreatedAt
                    });
                }

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for doctor: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(int patientId)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patientId && !a.IsDeleted)
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();

                var appointmentDtos = new List<AppointmentDto>();
                
                foreach (var appointment in appointments)
                {
                    var patient = await _patientClient.GetPatientAsync(appointment.PatientId);
                    var doctor = await _doctorClient.GetDoctorAsync(appointment.DoctorId);
                    
                    appointmentDtos.Add(new AppointmentDto
                    {
                        Id = appointment.Id,
                        PatientId = appointment.PatientId,
                        PatientName = patient?.Name ?? "Unknown",
                        DoctorId = appointment.DoctorId,
                        DoctorName = doctor?.Name ?? "Unknown",
                        DoctorSpecialization = doctor?.Specialization ?? "Unknown",
                        AppointmentDate = appointment.AppointmentDate,
                        Duration = appointment.Duration,
                        Status = appointment.Status,
                        CreatedAt = appointment.CreatedAt
                    });
                }

                return appointmentDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving appointments for patient: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate, int duration, int? excludeAppointmentId = null)
        {
            try
            {
                var endTime = appointmentDate.AddMinutes(duration);

                var conflictingAppointments = await _context.Appointments
                    .Where(a => a.DoctorId == doctorId 
                        && !a.IsDeleted 
                        && a.Status != AppointmentStatus.Cancelled
                        && a.Id != excludeAppointmentId)
                    .Where(a => 
                        (appointmentDate >= a.AppointmentDate && appointmentDate < a.AppointmentDate.AddMinutes(a.Duration)) ||
                        (endTime > a.AppointmentDate && endTime <= a.AppointmentDate.AddMinutes(a.Duration)) ||
                        (appointmentDate <= a.AppointmentDate && endTime >= a.AppointmentDate.AddMinutes(a.Duration)))
                    .AnyAsync();

                return !conflictingAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking doctor availability for Doctor: {DoctorId}", doctorId);
                throw;
            }
        }

        public async Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date, int duration = 30)
        {
            try
            {
                // For now, assume doctor is available - in real implementation, check doctor service

                var availableSlots = new List<DateTime>();
                var startTime = date.Date.AddHours(9); // Start at 9 AM
                var endTime = date.Date.AddHours(17); // End at 5 PM

                for (var currentTime = startTime; currentTime.AddMinutes(duration) <= endTime; currentTime = currentTime.AddMinutes(30))
                {
                    if (await IsDoctorAvailableAsync(doctorId, currentTime, duration))
                    {
                        availableSlots.Add(currentTime);
                    }
                }

                return availableSlots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available time slots for Doctor: {DoctorId} on {Date}", doctorId, date);
                throw;
            }
        }
    }
}
