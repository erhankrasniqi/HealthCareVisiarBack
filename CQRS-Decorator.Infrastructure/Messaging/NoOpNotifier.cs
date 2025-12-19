using CQRS_Decorator.SharedKernel; 

namespace CQRS_Decorator.Infrastructure.Messaging
{
    public class NoOpNotifier : INotifier
    {
        public Task Notify<TKey>(IAggregateRoot<TKey> notificationItem, string channelName)
        {
            throw new NotImplementedException();
        }
    }
}
