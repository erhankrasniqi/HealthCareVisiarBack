using System;
using System.Collections.Generic;
using System.Linq;
 
namespace CQRS_Decorator.SharedKernel
{
    public abstract class Entity<TKey> : IEntity<Guid>
    {
        public Guid Id { get; protected set; }
        public EntityStatus Status { get; protected set; } = EntityStatus.Active;
        public DateTime CreatedOn { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }

        public void SetCreatedOn(DateTime createdOn)
        {
            CreatedOn = (CreatedOn == default) ? createdOn : CreatedOn;
        }

        public void SetModifiedOn(DateTime modifiedOn)
        {
            ModifiedOn = modifiedOn;
        }

        public virtual void Delete()
        {
            Status = EntityStatus.Deleted;
        }

        public virtual void ThrowDomainException(string message)
        {
            throw new DomainException(message);
        }

        public virtual void ThrowDomainException(int errorCode, string message)
        {
            throw new DomainException(errorCode, message);
        }

        public virtual void ThrowDomainException(int errorCode, string message, Exception innerException)
        {
            throw new DomainException(errorCode, message, innerException);
        }
    }
}