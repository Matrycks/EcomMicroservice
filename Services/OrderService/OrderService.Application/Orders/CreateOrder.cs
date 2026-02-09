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

namespace OrderService.Application.Orders
{
    //public record CreateOrderCommand(Order NewOrder) : IRequest<Order>;

    public class CreateOrderHandler : IMessageHandler<CreateOrderMessage>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceBusPublisher _publisher;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository,
            IServiceBusPublisher publisher)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _publisher = publisher;
        }

        public async Task HandleTask(CreateOrderMessage message, CancellationToken cToken)
        {
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
        }
    }
}