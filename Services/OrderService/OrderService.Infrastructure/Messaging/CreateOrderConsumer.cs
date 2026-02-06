using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Common.Messaging.Orders;
using Mapster;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Messaging.Interfaces;

namespace OrderService.Infrastructure.Messaging
{
    public class CreateOrderConsumer : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IMessageDispatcher _dispatcher;
        private readonly ILogger<CreateOrderConsumer> _logger;

        public CreateOrderConsumer(ILogger<CreateOrderConsumer> logger,
            ServiceBusClient client, IMessageDispatcher dispatcher)
        {
            _logger = logger;

            _processor = client.CreateProcessor(
                topicName: "order-events",
                subscriptionName: "create-order",
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 2,
                    AutoCompleteMessages = false
                });

            _dispatcher = dispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += OnMessage;
            _processor.ProcessErrorAsync += OnError;

            await _processor.StartProcessingAsync(stoppingToken);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                // TODO: handle logging
                _logger.LogError(ex, "OrderMessageConsumer shutdown");
            }
        }

        private async Task OnMessage(ProcessMessageEventArgs args)
        {
            try
            {
                if (args.Message.Subject != nameof(CreateOrderMessage)) return;

                var msgEvent = JsonSerializer.Deserialize<CreateOrderMessage>(args.Message.Body);

                if (msgEvent == null) return;

                await _dispatcher.Dispatch<CreateOrderMessage>(msgEvent);

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message processing failed");
                await args.DeadLetterMessageAsync(args.Message,
                    "Process Error",
                    ex.Message);
            }
        }

        private async Task OnError(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
                "Service Bus error. Entity: {EntityPath}, ErrorSource: {ErrorSource}",
                args.EntityPath,
                args.ErrorSource);

            await Task.CompletedTask;
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();

            await base.StopAsync(cancellationToken);
        }
    }
}