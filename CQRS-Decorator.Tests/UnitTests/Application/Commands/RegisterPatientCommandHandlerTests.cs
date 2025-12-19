using CQRS_Decorator.Application.Common.Abstractions;
using CQRS_Decorator.Application.Features.Commands.RegisterPatient;
using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using Moq;
using Xunit;

namespace CQRS_Decorator.Tests.UnitTests.Application.Commands
{
    public class RegisterPatientCommandHandlerTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterPatientCommandHandler _handler;

        public RegisterPatientCommandHandlerTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _handler = new RegisterPatientCommandHandler(
                _patientRepositoryMock.Object,
                _passwordHasherMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldCreatePatient()
        {
            // Arrange
            var command = new RegisterPatientCommand(
                "John",
                "Doe",
                "john@email.com",
                "Password123!",
                "+1-555-1234",
                new DateTime(1990, 1, 1),
                "123 Main St");

            _patientRepositoryMock
                .Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync((Patient?)null);

            _passwordHasherMock
                .Setup(x => x.HashPassword(command.Password))
                .Returns("hashed_password");

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Patient registered successfully", result.Message);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithExistingEmail_ShouldReturnFailure()
        {
            // Arrange
            var command = new RegisterPatientCommand(
                "John",
                "Doe",
                "john@email.com",
                "Password123!",
                "+1-555-1234",
                new DateTime(1990, 1, 1),
                "123 Main St");

            var existingPatient = Patient.Create(
                "Jane",
                "Doe",
                "john@email.com",
                "+1-555-5678",
                new DateTime(1985, 1, 1),
                "456 Oak Ave",
                "hash");

            _patientRepositoryMock
                .Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync(existingPatient);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("A patient with this email already exists", result.Message);
            _patientRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_ShouldHashPassword()
        {
            // Arrange
            var command = new RegisterPatientCommand(
                "John",
                "Doe",
                "john@email.com",
                "Password123!",
                "+1-555-1234",
                new DateTime(1990, 1, 1),
                "123 Main St");

            _patientRepositoryMock
                .Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync((Patient?)null);

            _passwordHasherMock
                .Setup(x => x.HashPassword(command.Password))
                .Returns("hashed_password");

            // Act
            await _handler.HandleAsync(command);

            // Assert
            _passwordHasherMock.Verify(x => x.HashPassword(command.Password), Times.Once);
        }
    }
}
