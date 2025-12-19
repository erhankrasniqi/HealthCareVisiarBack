using CQRS_Decorator.Application.DTOs;
using CQRSDecorate.Net.Abstractions;

namespace CQRS_Decorator.Application.Features.Queries.GetPatientAppointments
{
    public record GetPatientAppointmentsQuery(Guid PatientId) : IQuery<IEnumerable<AppointmentReadModel>>;
}
