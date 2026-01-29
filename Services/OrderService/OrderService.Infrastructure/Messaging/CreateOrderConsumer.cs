using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Common.Messaging.Orders;
using Microsoft.Extensions.Hosting;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class CreateOrderConsumer : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IMessageDispatcher<CreateOrder> _dispatcher;

        public CreateOrderConsumer(ServiceBusClient client, IMessageDispatcher<CreateOrder> dispatcher)
        {
            _processor = client.CreateProcessor(
                topicName: "order-events",
                subscriptionName: "create-order",
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 3,
                    AutoCompleteMessages = false
                });

            _dispatcher = dispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _processor.ProcessMessageAsync += OnMessage;
                _processor.ProcessErrorAsync += _ => Task.CompletedTask; // TODO: add logging

                await _processor.StartProcessingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                // TODO: handle logging
                Console.WriteLine($"Error consuming message: {ex.Message}");
            }
        }

        private async Task OnMessage(ProcessMessageEventArgs args)
        {
            if (args.Message.Subject != nameof(CreateOrder)) return;

            var msgEvent = JsonSerializer.Deserialize<CreateOrder>(args.Message.Body);

            if (msgEvent == null) return;

            await _dispatcher.Dispatch(msgEvent);

            await args.CompleteMessageAsync(args.Message);
        }
    }
}