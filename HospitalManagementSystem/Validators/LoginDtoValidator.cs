using FluentValidation;
using HospitalManagementSystem.Models.DTOs;

namespace HospitalManagementSystem.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");
        }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format")
                .MaximumLength(100)
                .WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required")
                .Must(role => new[] { "Admin", "Doctor", "Patient", "Pharmacist", "Finance" }.Contains(role))
                .WithMessage("Invalid role specified");
        }
    }
}
