using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders
{
    public record GetOrderRequest(int OrderId) : IRequest<Order>;

    public class GetOrder : IRequestHandler<GetOrderRequest, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrder(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            if (request.OrderId <= 0) throw new ArgumentException("OrderId must be greater than zero");

            var order = await _orderRepository.GetAsync(request.OrderId)
                ?? throw new KeyNotFoundException($"Order: {request.OrderId} doesn't exist");

            return order;
        }
    }
}