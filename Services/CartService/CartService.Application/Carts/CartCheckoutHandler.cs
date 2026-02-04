using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Application.Messaging;
using Common.Messaging.Orders;
using Common.Messaging.Payloads;
using Mapster;
using MediatR;

namespace CartService.Application.Carts
{
    public record CartCheckoutCommand(int CartId, int PaymentCardId) : IRequest;

    public class CartCheckoutHandler : IRequestHandler<CartCheckoutCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IServiceBusPublisher _servieBusPublisher;

        public CartCheckoutHandler(ICartRepository cartRepository,
            IServiceBusPublisher servieBusPublisher)
        {
            _cartRepository = cartRepository;
            _servieBusPublisher = servieBusPublisher;
        }

        public async Task Handle(CartCheckoutCommand request, CancellationToken cancellationToken)
        {
            int cartId = request.CartId;
            if (cartId <= 0 || request.PaymentCardId <= 0)
                throw new ArgumentException($"Invalid params on checkout");

            var cart = await _cartRepository.GetAsync(cartId)
                ?? throw new KeyNotFoundException($"No cart found for cartId: {cartId}");

            // TODO: handle payment

            await _servieBusPublisher.PublishAsync(new CreateOrderMessage
            {
                CustomerId = cart.CustomerId,
                Items = cart.Items.Adapt<List<OrderItem>>(),
                Total = cart.Total,
                CreatedDate = DateTime.UtcNow
            });
        }
    }
}