using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Application.Messaging.Interfaces;
using OrderService.Application.Orders;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<MessageDispatcher> _logger;

        public MessageDispatcher(IServiceProvider provider,
            ILogger<MessageDispatcher> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task Dispatch<T>(T message, CancellationToken cancellationToken = default)
        {
            using var scope = _provider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();

            await handler.HandleTask(message, cancellationToken);
        }
    }
}