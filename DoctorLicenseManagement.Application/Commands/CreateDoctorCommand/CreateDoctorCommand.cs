using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;

namespace DoctorLicenseManagement.Application.Commands.CreateDoctorCommand
{
    public class CreateDoctorCommand : IRequest<CreateDoctorCommandResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public LicenseStatus Status { get; set; }
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
                LicenseExpiryDate = DateTime.UtcNow,
                Status = LicenseStatus.Active
            };
            var id = await _repository.CreateAsync(newDoctor);

            if (id > 0)
            {
                return new CreateDoctorCommandResponse
                {
                    Id = id,
                    Success = true,
                    Message = $"Doctor is created with id {id} "
                };
            }
            else
            {
                return new CreateDoctorCommandResponse
                {
                    Success = false,
                    Message = "Failed to create Doctor"
                };
            }
        }
    }
}

    