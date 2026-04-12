using DoctorLicenseManagement.API.Controllers;
using DoctorLicenseManagement.Application.Commands.CreateDoctorCommand;
using DoctorLicenseManagement.Application.Commands.DeleteDoctorCommand;
using DoctorLicenseManagement.Application.Commands.UpdateDoctorCommand;
using DoctorLicenseManagement.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DoctorLicenseManagement.Tests.API.Controllers
{
    public class DoctorsControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly DoctorsController _controller;

        public DoctorsControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new DoctorsController(_mockMediator.Object);
        }

        #region Create Tests

        [Fact]
        public async Task Create_WithValidCommand_ShouldReturnOkResult()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. John Doe",
                Email = "john@example.com",
                Specialization = "Cardiology",
                LicenseNumber = "LIC001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new CreateDoctorCommandResponse
            {
                Success = true,
                Message = "Doctor created successfully",
                Id = 1
            };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().Be(response);
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Create_ShouldSendCommandToMediator()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. Test",
                Email = "test@example.com",
                Specialization = "Pediatrics",
                LicenseNumber = "LIC002",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new CreateDoctorCommandResponse { Success = true };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _controller.Create(command);

            // Assert
            _mockMediator.Verify(
                m => m.Send(command, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Create_WithFailureResponse_ShouldReturnOkWithFailureData()
        {
            // Arrange
            var command = new CreateDoctorCommand
            {
                FullName = "Dr. Failure",
                Email = "failure@example.com",
                Specialization = "General",
                LicenseNumber = "LIC003",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new CreateDoctorCommandResponse
            {
                Success = false,
                Error = "Duplicate license number"
            };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var returnedResponse = okResult.Value as CreateDoctorCommandResponse;
            returnedResponse.Success.Should().BeFalse();
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_WithValidCommand_ShouldReturnOkResult()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. Updated",
                Email = "updated@example.com",
                Specialization = "Orthopedics",
                LicenseNumber = "LIC001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new UpdateDoctorCommandResponse
            {
                Success = true,
                Message = "Doctor updated successfully"
            };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Update_WithIdMismatch_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 2,
                FullName = "Dr. Updated",
                Email = "updated@example.com",
                Specialization = "Orthopedics",
                LicenseNumber = "LIC001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active
            };

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badResult = result as BadRequestObjectResult;
            badResult.Value.Should().BeOfType<string>().Which.Should().Contain("mismatch");
        }

        [Fact]
        public async Task Update_WithFailureResponse_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 999,
                FullName = "Dr. NonExistent",
                Email = "nonexistent@example.com",
                Specialization = "Surgery",
                LicenseNumber = "LIC999",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new UpdateDoctorCommandResponse
            {
                Success = false,
                Error = "Doctor not found"
            };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(999, command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_WithValidIdMatch_ShouldSendCommandToMediator()
        {
            // Arrange
            var command = new UpdateDoctorCommand
            {
                Id = 1,
                FullName = "Dr. Test",
                Email = "test@example.com",
                Specialization = "Cardiology",
                LicenseNumber = "LIC001",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                LicenseStatus = LicenseStatus.Active
            };

            var response = new UpdateDoctorCommandResponse { Success = true };

            _mockMediator
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _controller.Update(1, command);

            // Assert
            _mockMediator.Verify(
                m => m.Send(command, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var response = new DeleteDoctorCommandResponse
            {
                Success = true,
                Message = "Doctor deleted successfully"
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteDoctorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldReturnBadRequest()
        {
            // Arrange
            var response = new DeleteDoctorCommandResponse
            {
                Success = false,
                Error = "Doctor not found"
            };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteDoctorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_ShouldSendCorrectCommandToMediator()
        {
            // Arrange
            var response = new DeleteDoctorCommandResponse { Success = true };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteDoctorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _controller.Delete(1);

            // Assert
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<DeleteDoctorCommand>(cmd => cmd.Id == 1),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public async Task Delete_WithDifferentIds_ShouldDeleteCorrectDoctor(int id)
        {
            // Arrange
            var response = new DeleteDoctorCommandResponse { Success = true };

            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteDoctorCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            await _controller.Delete(id);

            // Assert
            _mockMediator.Verify(
                m => m.Send(
                    It.Is<DeleteDoctorCommand>(cmd => cmd.Id == id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
    }
}
