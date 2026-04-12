using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;

namespace DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand
{
    public class DeleteDoctorCommand : IRequest<DeleteDoctorCommandResponse>
    {
        public int Id { get; set; }
    }
    public class DeleteDoctorCommandResponse : ApiResponse
    { 
    }

    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, DeleteDoctorCommandResponse>
    {
        private readonly IDoctorRepository _repository;
        public DeleteDoctorCommandHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }
        public async Task<DeleteDoctorCommandResponse> Handle(DeleteDoctorCommand command,
            CancellationToken cancellationToken)
        {
             
            var result = await _repository.DeleteAsync(command.Id);

            if (result.Success)
            {
                return new DeleteDoctorCommandResponse
                { 
                    Success = true,
                    Message = result.Message
                };
            }
            else
            {
                return new DeleteDoctorCommandResponse
                {
                    Success = false,
                    Error = $"Doctor with id {command.Id} not found",
                    Message = result.Message
                };
            }
        }
    }
}

