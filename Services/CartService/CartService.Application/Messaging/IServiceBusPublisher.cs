using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Messaging
{
    public interface IServiceBusPublisher
    {
        public Task PublishAsync<T>(T message);
    }
}