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
            var updateDoctor = new Doctor
            {
                Id = command.Id,
                FullName = command.FullName,
                Email = command.Email,
                Specialization = command.Specialization,
                LicenseNumber = command.LicenseNumber,
                LicenseExpiryDate = command.LicenseExpiryDate,
                LicenseStatus = command.LicenseStatus
            };
            var result = await _repository.UpdateAsync(updateDoctor);

            if (result.Success)
            {
                return new UpdateDoctorCommandResponse
                {
                   
                    Success = true,
                    Message = result.Message
                };
            }
            else
            {
                return new UpdateDoctorCommandResponse
                {
                    Success = false,
                    Error = result.Message
                };
            }
        }
    }
}

    