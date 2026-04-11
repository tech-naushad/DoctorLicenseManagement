using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;
using System.Text.Json.Serialization;

namespace DoctorLicenseManagement.Application.Queries.GetAllDoctors
{
    public class GetAllDoctorsQuery : PageQuery,IRequest<GetAllDoctorsQueryResponse>
    {
        [JsonPropertyName("license_status")]
        public LicenseStatus? LicenseStatus { get; set; }       
    }

    public class GetAllDoctorsQueryResponse: ApiPageResponse
    {
        [JsonPropertyName("data")]
        public IEnumerable<DoctorResponse> Doctors { get; set; }
        
    }

    public class GetAllDoctorsQueryHandler
        : IRequestHandler<GetAllDoctorsQuery, GetAllDoctorsQueryResponse>
    {
        private readonly IDoctorRepository _repository;

        public GetAllDoctorsQueryHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAllDoctorsQueryResponse> Handle(GetAllDoctorsQuery query,
            CancellationToken cancellationToken)
        {
            var (doctors, totalCount) = await _repository.GetAllAsync
                (query.Search, query.LicenseStatus, query.Page, query.PageSize);

            var response= new GetAllDoctorsQueryResponse
            {
                Doctors = doctors.Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Email = d.Email,
                    Specialization = d.Specialization,
                    LicenseNumber = d.LicenseNumber,
                    LicenseExpiryDate = d.LicenseExpiryDate,
                    LicenseStatus = d.LicenseStatus
                })
            };
            response.Success = true;
            response.TotalCount = totalCount;
            response.Page = query.Page;
            response.PageSize = query.PageSize;
            response.TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            return response;
        }
    }
}