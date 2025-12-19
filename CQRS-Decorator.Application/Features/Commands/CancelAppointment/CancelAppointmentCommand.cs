using CQRS_Decorator.Application.Responses;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Commands.CancelAppointment
{
    public record CancelAppointmentCommand(Guid AppointmentId, Guid PatientId)
        : ICommand<GeneralResponse<bool>>;
}
