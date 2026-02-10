using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using MediatR;
using Mapster;
using OrderService.Application.Interfaces;
using OrderService.Application.Messaging.Interfaces;
using OrderService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Orders
{
    //public record CreateOrderCommand(Order NewOrder) : IRequest<Order>;

    public class CreateOrderHandler : IMessageHandler<CreateOrderMessage>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceBusPublisher _publisher;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository,
            IServiceBusPublisher publisher, ILogger<CreateOrderHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task HandleTask(CreateOrderMessage message, CancellationToken cToken)
        {
            _logger.LogInformation("*** CreateOrder started ***");

            var newOrder = new Order(message.CustomerId, new Guid(), message.Items.Adapt<List<OrderItem>>());

            await _orderRepository.AddAsync(newOrder);
            await _unitOfWork.SaveChangesAsync();

            // await _publisher.PublishAsync(new OrderCreatedMessage
            // {
            //     OrderId = newOrder.OrderId,
            //     CustomerId = newOrder.CustomerId,
            //     TotalAmount = newOrder.Total,
            //     CreatedDate = newOrder.CreatedDate
            // });

            _logger.LogInformation("*** CreateOrder finished ***");
        }
    }
}