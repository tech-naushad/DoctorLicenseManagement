using DoctorLicenseManagement.Application.Queries.GetDoctorsById;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
 

namespace DoctorLicenseManagement.Tests
{
    public class GetDoctorByIdQueryHandlerTests
    {
        private readonly Mock<IDoctorRepository> _mockRepository;
        private readonly GetDoctorsByIdQueryHandler _handler;

        public GetDoctorByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IDoctorRepository>();
            _handler = new GetDoctorsByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnDoctor()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. John Doe",
                Email = "john@example.com",
                Specialization = "Cardiology",
                LicenseNumber = "LIC001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                LicenseStatus = LicenseStatus.Active,
                CreatedDate = DateTime.UtcNow
            };

            var query = new GetDoctorsByIdQuery { Id = 1 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Doctor.Should().NotBeNull();
            result.Doctor.Id.Should().Be(1);
            result.Doctor.FullName.Should().Be("Dr. John Doe");
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldReturnFailureResponse()
        {
            
            // Arrange
            var query = new GetDoctorsByIdQuery { Id = 999 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Doctor)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("not found");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task Handle_WithDifferentIds_ShouldCallRepositoryWithCorrectId(int id)
        {
            // Arrange
            var query = new GetDoctorsByIdQuery { Id = id };
            var doctor = new Doctor { Id = id, FullName = $"Dr. {id}" };

            _mockRepository
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(doctor);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldMapDoctorEntityToResponse()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 1,
                FullName = "Dr. Mapping Test",
                Email = "mapping@example.com",
                Specialization = "Pediatrics",
                LicenseNumber = "LICMAP",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active,
                CreatedDate = DateTime.UtcNow
            };

            var query = new GetDoctorsByIdQuery { Id = 1 };

            _mockRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var doctorResponse = result.Doctor;
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
