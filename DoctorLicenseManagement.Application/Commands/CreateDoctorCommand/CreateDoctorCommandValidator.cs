using FluentValidation;

namespace DoctorLicenseManagement.Application.Commands.CreateDoctorCommand
{
    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().NotNull().WithMessage("Full Name is required");
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
            RuleFor(x => x.Specialization).NotNull().WithMessage("Specialization is required");
            RuleFor(x => x.LicenseNumber).NotNull().WithMessage("License Number is required");
            RuleFor(x => x.LicenseStatus).NotNull().WithMessage("License staus is required");
        }
    }
}
