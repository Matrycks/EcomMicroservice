using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Application.Messaging.Interfaces
{
    public interface IMessageHandler<in T>
    {
        Task HandleTask(T message, CancellationToken cToken);
    }
}