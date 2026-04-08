using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;

namespace DoctorLicenseManagement.Application.Commands.CreateDoctorCommand
{
    public class UpdateDoctorCommand : DoctorCommand,IRequest<UpdateDoctorCommandResponse>
    {
        public int Id { get; set; }        
    }
    public class UpdateDoctorCommandResponse : ApiResponse
    {
        public int Id { get; set; }
    }

    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, UpdateDoctorCommandResponse>
    {
        private readonly IDoctorRepository _repository;
        public UpdateDoctorCommandHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }
        public async Task<UpdateDoctorCommandResponse> Handle(UpdateDoctorCommand command,
            CancellationToken cancellationToken)
        {
            var newDoctor = new Doctor
            {
                FullName = command.FullName,
                Email = command.Email,
                Specialization = command.Specialization,
                LicenseNumber = command.LicenseNumber,
                LicenseExpiryDate = DateTime.UtcNow,
                Status = LicenseStatus.Active
            };
            var result = await _repository.UpdateAsync(newDoctor);

            if (result)
            {
                return new UpdateDoctorCommandResponse
                {
                   
                    Success = true,
                    Message = "Recored Updated Successfully"
                };
            }
            else
            {
                return new UpdateDoctorCommandResponse
                {
                    Success = false,
                    Message = "Failed to update record"
                };
            }
        }
    }
}

    