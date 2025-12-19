namespace CQRS_Decorator.Application.DTOs
{
    public class AppointmentReadModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string? Notes { get; set; } // Nullable - only filled when doctor completes appointment
        public DateTime CreatedOn { get; set; }
    }
}
