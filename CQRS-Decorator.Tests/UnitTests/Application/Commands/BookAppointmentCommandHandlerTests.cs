using CQRS_Decorator.Application.Features.Commands.BookAppointment;
using CQRS_Decorator.Domain.Aggregates.AppointmentAggregate;
using CQRS_Decorator.Domain.Aggregates.DoctorAggregate;
using CQRS_Decorator.Domain.Aggregates.PatientAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using Moq;
using Xunit;

namespace CQRS_Decorator.Tests.UnitTests.Application.Commands
{
    public class BookAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly BookAppointmentCommandHandler _handler;

        public BookAppointmentCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _handler = new BookAppointmentCommandHandler(
                _appointmentRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldBookAppointment()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var command = new BookAppointmentCommand(
                patientId,
                doctorId,
                DateTime.Today.AddDays(7),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                "Annual checkup");

            var patient = Patient.Create(
                "John", "Doe", "john@email.com",
                "+1-555-1234", new DateTime(1990, 1, 1), "123 Main St", "hash");

            var doctor = Doctor.Create(
                "Dr. Smith", "John", "Cardiology",
                "dr.smith@hospital.com", "+1-555-9999", "LIC123", 15);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x => x.IsDoctorAvailableAsync(
                    doctorId, command.AppointmentDate, command.StartTime, command.EndTime))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Appointment booked successfully", result.Message);
            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithNonExistentPatient_ShouldReturnFailure()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var command = new BookAppointmentCommand(
                patientId,
                doctorId,
                DateTime.Today.AddDays(7),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                "Annual checkup");

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(patientId))
                .ReturnsAsync((Patient?)null);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Patient not found", result.Message);
            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WithUnavailableDoctor_ShouldReturnFailure()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var command = new BookAppointmentCommand(
                patientId,
                doctorId,
                DateTime.Today.AddDays(7),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                "Annual checkup");

            var patient = Patient.Create(
                "John", "Doe", "john@email.com",
                "+1-555-1234", new DateTime(1990, 1, 1), "123 Main St", "hash");

            var doctor = Doctor.Create(
                "Dr. Smith", "John", "Cardiology",
                "dr.smith@hospital.com", "+1-555-9999", "LIC123", 15);
            doctor.SetAvailability(false);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Doctor is not available", result.Message);
            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WithDoctorTimeConflict_ShouldReturnFailure()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var command = new BookAppointmentCommand(
                patientId,
                doctorId,
                DateTime.Today.AddDays(7),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                "Annual checkup");

            var patient = Patient.Create(
                "John", "Doe", "john@email.com",
                "+1-555-1234", new DateTime(1990, 1, 1), "123 Main St", "hash");

            var doctor = Doctor.Create(
                "Dr. Smith", "John", "Cardiology",
                "dr.smith@hospital.com", "+1-555-9999", "LIC123", 15);

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            _doctorRepositoryMock
                .Setup(x => x.GetByIdAsync(doctorId))
                .ReturnsAsync(doctor);

            _appointmentRepositoryMock
                .Setup(x => x.IsDoctorAvailableAsync(
                    doctorId, command.AppointmentDate, command.StartTime, command.EndTime))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.HandleAsync(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Doctor is not available at the selected time", result.Message);
            _appointmentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }
    }
}
