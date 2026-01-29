using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging.Orders;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders
{
    public record CreateOrderCommand(Order NewOrder) : IRequest<Order>;

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceBusPublisher _publisher;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IServiceBusPublisher publisher)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _publisher = publisher;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken = default)
        {
            //TODO: add validation
            var newOrder = request.NewOrder;

            await _orderRepository.AddAsync(newOrder);
            await _unitOfWork.SaveChangesAsync();

            await _publisher.PublishAsync(new OrderCreated
            {
                OrderId = newOrder.OrderId,
                CustomerId = newOrder.CustomerId,
                TotalAmount = newOrder.Total,
                CreatedDate = newOrder.CreatedDate
            });

            return newOrder;
        }
    }
}