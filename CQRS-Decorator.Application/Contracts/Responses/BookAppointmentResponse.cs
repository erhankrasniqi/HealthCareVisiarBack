namespace CQRS_Decorator.Application.Contracts.Responses
{
    public record BookAppointmentResponse(
        Guid AppointmentId,
        Guid PatientId,
        Guid DoctorId,
        string DoctorName,
        DateTime AppointmentDate,
        TimeSpan StartTime,
        TimeSpan EndTime,
        string Reason,
        string Status,
        DateTime CreatedAt);
}
