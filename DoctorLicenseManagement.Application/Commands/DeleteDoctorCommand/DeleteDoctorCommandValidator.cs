using FluentValidation;

namespace DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand
{
    public class DeleteDoctorCommandValidator : AbstractValidator<DeleteDoctorCommand>
    {
        public DeleteDoctorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Doctor Id is required");            
        }
    }
}
