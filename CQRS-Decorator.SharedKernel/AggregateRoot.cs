using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS_Decorator.SharedKernel
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<Guid>
    {
        private readonly List<object> _domainEvents = new();
        
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
