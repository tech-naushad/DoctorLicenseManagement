using DoctorLicenseManagement.Application.Queries.GetAllDoctors;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace DoctorLicenseManagement.Tests.Application.Queries
{
    public class GetAllDoctorsQueryHandlerTests
    {
        private readonly Mock<IDoctorRepository> _mockRepository;
        private readonly GetAllDoctorsQueryHandler _handler;

        public GetAllDoctorsQueryHandlerTests()
        {
            _mockRepository = new Mock<IDoctorRepository>();
            _handler = new GetAllDoctorsQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidQuery_ShouldReturnDoctorsList()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    FullName = "Dr. John Doe",
                    Email = "john@example.com",
                    Specialization = "Cardiology",
                    LicenseNumber = "LIC001",
                    LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                    LicenseStatus = LicenseStatus.Active,
                    CreatedDate = DateTime.UtcNow
                },
                new Doctor
                {
                    Id = 2,
                    FullName = "Dr. Jane Smith",
                    Email = "jane@example.com",
                    Specialization = "Neurology",
                    LicenseNumber = "LIC002",
                    LicenseExpiryDate = DateTime.UtcNow.AddMonths(6),
                    LicenseStatus = LicenseStatus.Active,
                    CreatedDate = DateTime.UtcNow
                }
            };

            var query = new GetAllDoctorsQuery
            {
                Page = 1,
                PageSize = 10,
                Search = null,
                LicenseStatus = null
            };

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<string>(), It.IsAny<LicenseStatus?>(), 
                    It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((doctors, doctors.Count));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Doctors.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Handle_WithSearchFilter_ShouldReturnFilteredDoctors()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    FullName = "Dr. John Doe",
                    Email = "john@example.com",
                    Specialization = "Cardiology",
                    LicenseNumber = "LIC001",
                    LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                    LicenseStatus = LicenseStatus.Active,
                    CreatedDate = DateTime.UtcNow
                }
            };

            var query = new GetAllDoctorsQuery
            {
                Page = 1,
                PageSize = 10,
                Search = "John",
                LicenseStatus = null
            };

            _mockRepository
                .Setup(r => r.GetAllAsync("John", null, 1, 10))
                .ReturnsAsync((doctors, 1));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Doctors.Should().HaveCount(1);
            result.Doctors.First().FullName.Should().Contain("John");
        }

        [Fact]
        public async Task Handle_WithLicenseStatusFilter_ShouldReturnFilteredByStatus()
        {
            // Arrange
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Id = 3,
                    FullName = "Dr. Expired License",
                    Email = "expired@example.com",
                    Specialization = "Pediatrics",
                    LicenseNumber = "LIC003",
                    LicenseExpiryDate = DateTime.UtcNow.AddMonths(-1),
                    LicenseStatus = LicenseStatus.Expired,
                    CreatedDate = DateTime.UtcNow
                }
            };

            var query = new GetAllDoctorsQuery
            {
                Page = 1,
                PageSize = 10,
                Search = null,
                LicenseStatus = LicenseStatus.Expired
            };

            _mockRepository
                .Setup(r => r.GetAllAsync(null, LicenseStatus.Expired, 1, 10))
                .ReturnsAsync((doctors, 1));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Doctors.Should().HaveCount(1);
            result.Doctors.First().LicenseStatus.Should().Be(LicenseStatus.Expired);
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetAllDoctorsQuery
            {
                Page = 1,
                PageSize = 10,
                Search = "NonExistent",
                LicenseStatus = null
            };

            _mockRepository
                .Setup(r => r.GetAllAsync("NonExistent", null, 1, 10))
                .ReturnsAsync((new List<Doctor>(), 0));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Doctors.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
        }

        [Fact]
        public async Task Handle_ShouldCalculatePaginationCorrectly()
        {
            // Arrange
            var doctors = Enumerable.Range(1, 5)
                .Select(i => new Doctor
                {
                    Id = i,
                    FullName = $"Dr. {i}",
                    Email = $"doctor{i}@example.com",
                    Specialization = "General",
                    LicenseNumber = $"LIC{i:00000}",
                    LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                    LicenseStatus = LicenseStatus.Active,
                    CreatedDate = DateTime.UtcNow
                }).ToList();

            var query = new GetAllDoctorsQuery
            {
                Page = 2,
                PageSize = 2
            };

            _mockRepository
                .Setup(r => r.GetAllAsync(null, null, 2, 2))
                .ReturnsAsync((doctors.Skip(2).Take(2).ToList(), 5));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.TotalPages.Should().Be(3); // Ceiling(5 / 2) = 3
            result.Page.Should().Be(2);
            result.PageSize.Should().Be(2);
            result.TotalCount.Should().Be(5);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        [InlineData(3, 20)]
        public async Task Handle_WithDifferentPaginationParameters_ShouldReturnCorrectPageInfo(
            int page, int pageSize)
        {
            // Arrange
            var doctors = new List<Doctor>();
            var query = new GetAllDoctorsQuery
            {
                Page = page,
                PageSize = pageSize
            };

            _mockRepository
                .Setup(r => r.GetAllAsync(null, null, page, pageSize))
                .ReturnsAsync((doctors, 100));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public async Task Handle_ShouldMapDoctorEntitiesToResponses()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Mapping Test",
                Email = "mapping@example.com",
                Specialization = "Orthopedics",
                LicenseNumber = "LICMAP001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active,
                CreatedDate = DateTime.UtcNow
            };

            var query = new GetAllDoctorsQuery { Page = 1, PageSize = 10 };

            _mockRepository
                .Setup(r => r.GetAllAsync(null, null, 1, 10))
                .ReturnsAsync((new List<Doctor> { doctor }, 1));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var doctorResponse = result.Doctors.First();
            doctorResponse.Id.Should().Be(doctor.Id);
            doctorResponse.FullName.Should().Be(doctor.FullName);
            doctorResponse.Email.Should().Be(doctor.Email);
            doctorResponse.Specialization.Should().Be(doctor.Specialization);
            doctorResponse.LicenseNumber.Should().Be(doctor.LicenseNumber);
            doctorResponse.LicenseExpiryDate.Should().Be(doctor.LicenseExpiryDate);
            doctorResponse.LicenseStatus.Should().Be(doctor.LicenseStatus);
        }
    }
}
