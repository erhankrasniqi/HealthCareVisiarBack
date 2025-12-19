using CQRS_Decorator.Application.Responses;
using CQRS_Decorator.Domain.Aggregates.AppointmentAggregate;
using CQRS_Decorator.Application.Common.Repositories;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.BookAppointment
{
    public class BookAppointmentCommandHandler : ICommandHandler<BookAppointmentCommand, GeneralResponse<Guid>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public BookAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<GeneralResponse<Guid>> HandleAsync(BookAppointmentCommand command)
        {
            var patient = await _patientRepository.GetByIdAsync(command.PatientId);
            if (patient == null)
            {
                return new GeneralResponse<Guid>
                {
                    Success = false,
                    Message = "Patient not found"
                };
            }

            var doctor = await _doctorRepository.GetByIdAsync(command.DoctorId);
            if (doctor == null)
            {
                return new GeneralResponse<Guid>
                {
                    Success = false,
                    Message = "Doctor not found"
                };
            }

            if (!doctor.IsAvailable)
            {
                return new GeneralResponse<Guid>
                {
                    Success = false,
                    Message = "Doctor is not available"
                };
            }

            var isDoctorAvailable = await _appointmentRepository.IsDoctorAvailableAsync(
                command.DoctorId,
                command.AppointmentDate,
                command.StartTime,
                command.EndTime);

            if (!isDoctorAvailable)
            {
                return new GeneralResponse<Guid>
                {
                    Success = false,
                    Message = "Doctor is not available at the selected time"
                };
            }

            var appointment = Appointment.Create(
                command.PatientId,
                command.DoctorId,
                command.AppointmentDate,
                command.StartTime,
                command.EndTime,
                command.Reason);
            try
            {
                await _appointmentRepository.AddAsync(appointment);
            }
            catch(Exception ex)
            {
                throw;
            }
           

            return new GeneralResponse<Guid>
            {
                Success = true,
                Message = "Appointment booked successfully",
                Result = appointment.Id
            };
        }
    }
}
