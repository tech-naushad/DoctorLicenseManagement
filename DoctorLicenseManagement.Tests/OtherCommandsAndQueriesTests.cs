using DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand;
using DoctorLicenseManagement.Application.Commands.UpdateDoctorCommand;
using DoctorLicenseManagement.Domain.Entities;
using DoctorLicenseManagement.Domain.Enums;
using DoctorLicenseManagement.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace DoctorLicenseManagement.Tests.Application
{
    public class UpdateDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorRepository> _mockRepository;
        private readonly UpdateDoctorCommandHandler _handler;

        public UpdateDoctorCommandHandlerTests()
        {
            _mockRepository = new Mock<IDoctorRepository>();
            _handler = new UpdateDoctorCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. Updated Name",
                Email = "updated@example.com",
                Specialization = "Surgery",
                LicenseNumber = "LICUPD001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active
            };

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                .ReturnsAsync((true, "Doctor updated successfully"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Doctor updated successfully");
        }

        [Fact]
        public async Task Handle_WithNonExistentDoctor_ShouldReturnFailureResponse()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 999,
                FullName = "Dr. Ghost",
                Email = "ghost@example.com",
                Specialization = "General",
                LicenseNumber = "LICGHOST",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                LicenseStatus = LicenseStatus.Active
            };

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                .ReturnsAsync((false, "Doctor not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
            result.Error.Should().Be("Doctor not found");
        }

        [Fact]
        public async Task Handle_ShouldMapCommandToDoctorEntity()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. Mapped",
                Email = "mapped@example.com",
                Specialization = "Neurology",
                LicenseNumber = "LICMAPPED",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(3),
                LicenseStatus = LicenseStatus.Expired
            };

            Doctor capturedDoctor = null;
            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Doctor>()))
                .Callback<Doctor>(d => capturedDoctor = d)
                .ReturnsAsync((true, "Success"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            capturedDoctor.Should().NotBeNull();
            capturedDoctor.Id.Should().Be(command.Id);
            capturedDoctor.FullName.Should().Be(command.FullName);
            capturedDoctor.Email.Should().Be(command.Email);
        }
    }

    public class DeleteDoctorCommandHandlerTests
    {
        private readonly Mock<IDoctorRepository> _mockRepository;
        private readonly DeleteDoctorCommandHandler _handler;

        public DeleteDoctorCommandHandlerTests()
        {
            _mockRepository = new Mock<IDoctorRepository>();
            _handler = new DeleteDoctorCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = new DeleteDoctorCommand { Id = 1 };

            _mockRepository
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync((true, "Doctor deleted successfully"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Doctor deleted successfully");
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldReturnFailureResponse()
        {
            // Arrange
            var command = new DeleteDoctorCommand { Id = 999 };

            _mockRepository
                .Setup(r => r.DeleteAsync(999))
                .ReturnsAsync((false, "Doctor not found"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public async Task Handle_WithDifferentIds_ShouldCallRepositoryWithCorrectId(int id)
        {
            // Arrange
            var command = new DeleteDoctorCommand { Id = id };

            _mockRepository
                .Setup(r => r.DeleteAsync(id))
                .ReturnsAsync((true, "Success"));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }

    
}
