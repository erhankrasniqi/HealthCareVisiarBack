using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Decorator.SharedKernel
{
    public interface INotifier
    {
        Task Notify<TKey>(IAggregateRoot<TKey> notificationItem, string channelName);
    }
}
