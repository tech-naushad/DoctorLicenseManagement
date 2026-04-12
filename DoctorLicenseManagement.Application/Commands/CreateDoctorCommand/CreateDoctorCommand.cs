using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;
using System.Text.Json.Serialization;

namespace DoctorLicenseManagement.Application.Commands.CreateDoctorCommand
{
    public class CreateDoctorCommand : DoctorCommand,IRequest<CreateDoctorCommandResponse>
    {       
    }
    public class CreateDoctorCommandResponse : ApiResponse
    {
        public int Id { get; set; }
    }

    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, CreateDoctorCommandResponse>
    {
        private readonly IDoctorRepository _repository;
        public CreateDoctorCommandHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }
        public async Task<CreateDoctorCommandResponse> Handle(CreateDoctorCommand command,
            CancellationToken cancellationToken)
        {
            var newDoctor = new Doctor
            {
                FullName = command.FullName,
                Email = command.Email,
                Specialization = command.Specialization,
                LicenseNumber = command.LicenseNumber,
                LicenseExpiryDate = command.LicenseExpiryDate,
                LicenseStatus = command.LicenseStatus
            };
            var result = await _repository.CreateAsync(newDoctor);

            if (result.Success)
            {
                return new CreateDoctorCommandResponse
                {                    
                    Success = true,
                    Message = result.Message
                };
            }
            else
            {
                return new CreateDoctorCommandResponse
                {
                    Success = false,
                    Error = result.Message
                };
            }
        }
    }
}

