using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Application.Queries.Common;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;
using System.Text.Json.Serialization;

namespace DoctorLicenseManagement.Application.Queries.GetDoctorsById
{
    public class GetDoctorsByIdQuery : IRequest<GetDoctorsByIdResponse>
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }       
    }

    public class GetDoctorsByIdResponse : ApiResponse
    {
        [JsonPropertyName("data")]
        public DoctorResponse Doctor { get; set; }        
    }

    public class GetDoctorsByIdQueryHandler
        : IRequestHandler<GetDoctorsByIdQuery, GetDoctorsByIdResponse>
    {
        private readonly IDoctorRepository _repository;

        public GetDoctorsByIdQueryHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetDoctorsByIdResponse> Handle(GetDoctorsByIdQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _repository.GetByIdAsync
                (query.Id);

            var response= new GetDoctorsByIdResponse
            {
                Doctor = new DoctorResponse
                {
                    Id = result.Id,
                    FullName = result.FullName,
                    Email = result.Email,
                    Specialization = result.Specialization,
                    LicenseNumber = result.LicenseNumber,
                    LicenseExpiryDate = result.LicenseExpiryDate,
                    LicenseStatus = result.LicenseStatus
                }
            };
            response.Success = true;           
            return response;
        }
    }
}