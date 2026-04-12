using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorLicenseManagement.Application.Commands.UpdateDoctorCommand
{
    public class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdateDoctorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Doctor Id is required");
            RuleFor(x => x.FullName).NotEmpty().NotNull().WithMessage("Full Name is required");
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
            RuleFor(x => x.Specialization).NotNull().WithMessage("Specialization is required");
            RuleFor(x => x.LicenseNumber).NotNull().WithMessage("License Number is required");
            RuleFor(x => x.LicenseStatus).NotNull().WithMessage("License staus is required");
        }
    }
}
