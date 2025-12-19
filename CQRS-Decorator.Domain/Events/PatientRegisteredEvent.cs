namespace CQRS_Decorator.Domain.Events
{
    public class PatientRegisteredEvent : DomainEvent
    {
        public Guid PatientId { get; }
        public string Email { get; }
        public string FullName { get; }

        public PatientRegisteredEvent(Guid patientId, string email, string fullName)
        {
            PatientId = patientId;
            Email = email;
            FullName = fullName;
        }
    }
}
