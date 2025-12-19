namespace CQRS_Decorator.Domain.Events
{
    public class AppointmentCancelledEvent : DomainEvent
    {
        public Guid AppointmentId { get; }
        public Guid PatientId { get; }
        public Guid DoctorId { get; }
        public DateTime AppointmentDate { get; }
        public string CancellationReason { get; }

        public AppointmentCancelledEvent(
            Guid appointmentId,
            Guid patientId,
            Guid doctorId,
            DateTime appointmentDate,
            string cancellationReason = null)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            CancellationReason = cancellationReason;
        }
    }
}
