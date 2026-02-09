using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Messaging.Interfaces;
using OrderService.Application.Orders;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceProvider _provider;

        public MessageDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Dispatch<T>(T message, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();

            await handler.HandleTask(message, cancellationToken);
        }
    }
}