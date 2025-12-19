using CQRS_Decorator.Domain.Events;
using CQRS_Decorator.Domain.Exceptions;
using CQRS_Decorator.SharedKernel;

namespace CQRS_Decorator.Domain.Aggregates.AppointmentAggregate
{
    public enum AppointmentStatus
    {
        Scheduled = 1,
        Completed = 2,
        Cancelled = 3,
        NoShow = 4
    }

    public class Appointment : AggregateRoot<int>
    {
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime AppointmentDate { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public AppointmentStatus AppointmentStatus { get; private set; }
        public string Reason { get; private set; }
        public string? Notes { get; private set; }

        private Appointment() { }

        private Appointment(
            Guid patientId,
            Guid doctorId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            string reason)
        {
            if (patientId == Guid.Empty)
                throw new ValidationException("Patient ID is required");
            if (doctorId == Guid.Empty)
                throw new ValidationException("Doctor ID is required");
            if (appointmentDate < DateTime.Today)
                throw new BusinessRuleViolationException("Appointment date cannot be in the past");
            if (startTime >= endTime)
                throw new ValidationException("Start time must be before end time");

            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            EndTime = endTime;
            AppointmentStatus = AppointmentStatus.Scheduled;
            Reason = reason;
            Notes = null;

            // Raise domain event
            AddDomainEvent(new AppointmentBookedEvent(
                Id, patientId, doctorId, appointmentDate, startTime, endTime, reason));
        }

        public static Appointment Create(
            Guid patientId,
            Guid doctorId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            string reason)
            => new(patientId, doctorId, appointmentDate, startTime, endTime, reason);

        public void Cancel()
        {
            if (AppointmentStatus == AppointmentStatus.Cancelled)
                throw new BusinessRuleViolationException("Appointment is already cancelled");

            if (AppointmentStatus == AppointmentStatus.Completed)
                throw new BusinessRuleViolationException("Cannot cancel a completed appointment");

            AppointmentStatus = AppointmentStatus.Cancelled;
            SetModifiedOn(DateTime.UtcNow);

            // Raise domain event
            AddDomainEvent(new AppointmentCancelledEvent(
                Id, PatientId, DoctorId, AppointmentDate));
        }

        public void Complete(string? notes = null)
        {
            if (AppointmentStatus == AppointmentStatus.Cancelled)
                throw new BusinessRuleViolationException("Cannot complete a cancelled appointment");

            AppointmentStatus = AppointmentStatus.Completed;
            Notes = notes;
            SetModifiedOn(DateTime.UtcNow);
        }

        public void MarkAsNoShow()
        {
            AppointmentStatus = AppointmentStatus.NoShow;
            SetModifiedOn(DateTime.UtcNow);
        }

        public void Reschedule(DateTime newDate, TimeSpan newStartTime, TimeSpan newEndTime)
        {
            if (AppointmentStatus == AppointmentStatus.Cancelled)
                throw new BusinessRuleViolationException("Cannot reschedule a cancelled appointment");

            if (newDate < DateTime.Today)
                throw new BusinessRuleViolationException("New appointment date cannot be in the past");

            AppointmentDate = newDate;
            StartTime = newStartTime;
            EndTime = newEndTime;
            SetModifiedOn(DateTime.UtcNow);
        }

        public bool IsPast() => AppointmentDate < DateTime.Today;
        
        public bool IsToday() => AppointmentDate.Date == DateTime.Today;

        public TimeSpan GetDuration() => EndTime - StartTime;
    }
}
