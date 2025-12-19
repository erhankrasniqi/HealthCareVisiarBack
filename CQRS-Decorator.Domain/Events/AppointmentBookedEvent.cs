namespace CQRS_Decorator.Domain.Events
{
    public class AppointmentBookedEvent : DomainEvent
    {
        public Guid AppointmentId { get; }
        public Guid PatientId { get; }
        public Guid DoctorId { get; }
        public DateTime AppointmentDate { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Reason { get; }

        public AppointmentBookedEvent(
            Guid appointmentId,
            Guid patientId,
            Guid doctorId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            string reason)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            EndTime = endTime;
            Reason = reason;
        }
    }
}
