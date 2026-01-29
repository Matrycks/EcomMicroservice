using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class OrderMessagePublisher : IServiceBusPublisher
    {
        private readonly ServiceBusSender _sender;

        public OrderMessagePublisher(ServiceBusClient client)
        {
            _sender = client.CreateSender("order-events");
        }

        public async Task PublishAsync<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);

            var sbMessage = new ServiceBusMessage(json)
            {
                Subject = typeof(T).Name,
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(sbMessage);
        }
    }
}