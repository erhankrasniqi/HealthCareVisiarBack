using CQRS_Decorator.Application.Responses;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.BookAppointment
{
    public record BookAppointmentCommand(
        Guid PatientId,
        Guid DoctorId,
        DateTime AppointmentDate,
        TimeSpan StartTime,
        TimeSpan EndTime,
        string Reason
    ) : ICommand<GeneralResponse<Guid>>;
}
