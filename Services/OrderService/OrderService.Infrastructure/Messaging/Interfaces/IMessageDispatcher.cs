using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Messaging.Interfaces
{
    public interface IMessageDispatcher
    {
        public Task Dispatch<T>(T message, CancellationToken cancellationToken = default);
    }
}