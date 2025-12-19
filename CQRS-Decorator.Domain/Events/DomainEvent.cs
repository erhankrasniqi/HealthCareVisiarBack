namespace CQRS_Decorator.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
        Guid EventId { get; }
    }

    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; }
        public Guid EventId { get; }

        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }
    }
}
