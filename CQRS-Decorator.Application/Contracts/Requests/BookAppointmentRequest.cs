using System.ComponentModel.DataAnnotations;

namespace CQRS_Decorator.Application.Contracts.Requests
{
    public record BookAppointmentRequest(
        [Required] Guid DoctorId,
        [Required] DateTime AppointmentDate,
        [Required] TimeSpan StartTime,
        [Required] TimeSpan EndTime,
        [Required] string Reason);
}
