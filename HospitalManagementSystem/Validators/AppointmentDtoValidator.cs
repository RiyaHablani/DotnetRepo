using FluentValidation;
using HospitalManagementSystem.Models.DTOs;

namespace HospitalManagementSystem.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("Patient ID must be greater than 0");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .WithMessage("Doctor ID must be greater than 0");

            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment date must be in the future");

            RuleFor(x => x.Duration)
                .InclusiveBetween(15, 120)
                .WithMessage("Duration must be between 15 and 120 minutes");
        }
    }

    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Appointment date must be in the future")
                .When(x => x.AppointmentDate.HasValue);

            RuleFor(x => x.Duration)
                .InclusiveBetween(15, 120)
                .WithMessage("Duration must be between 15 and 120 minutes")
                .When(x => x.Duration.HasValue);
        }
    }
}
