using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders
{
    public record GetOrdersRequest() : IRequest<IEnumerable<Order>>;

    public class GetOrders : IRequestHandler<GetOrdersRequest, IEnumerable<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrders(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders;
        }
    }
}