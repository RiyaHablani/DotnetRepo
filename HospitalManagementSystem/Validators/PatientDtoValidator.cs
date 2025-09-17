using FluentValidation;
using HospitalManagementSystem.Models.DTOs;

namespace HospitalManagementSystem.Validators
{
    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0")
                .LessThanOrEqualTo(150).WithMessage("Age must be less than or equal to 150");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(x => x.ToLower() == "male" || x.ToLower() == "female" || x.ToLower() == "other")
                .WithMessage("Gender must be Male, Female, or Other");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters");
        }
    }
}
