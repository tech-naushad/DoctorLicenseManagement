using DoctorLicenseManagement.Application.Common;
using DoctorLicenseManagement.Infrastructure.Repositories;
using MediatR;

namespace DoctorLicenseManagement.Application.Queries
{
    public class GetAllDoctorsQuery : IRequest<GetAllDoctorsQueryResponse>
    {
    }

    public class GetAllDoctorsQueryResponse
    {
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

        public async Task<GetAllDoctorsQueryResponse> Handle(GetAllDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            var doctors = await _repository.GetAllAsync();

            return new GetAllDoctorsQueryResponse
            {
                Doctors = doctors.Select(d => new DoctorResponse
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Email = d.Email,
                    Specialization = d.Specialization,
                    LicenseNumber = d.LicenseNumber,
                    LicenseExpiryDate = d.LicenseExpiryDate,
                    Status = d.Status
                })
            };
        }
    }
}