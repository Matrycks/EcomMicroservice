using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Orders;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class CreateOrderMessageDispatcher : IMessageDispatcher<CreateOrder>
    {
        private readonly IServiceProvider _provider;

        public CreateOrderMessageDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Dispatch(CreateOrder evt)
        {
            using var scope = _provider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<CreateOrderHandler>();

            await handler.Handle(new CreateOrderCommand(evt.Adapt<Order>()));
        }
    }
}