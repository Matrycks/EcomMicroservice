using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using MediatR;

namespace CartService.Application.Carts
{
    public record CreateCartCommand(int CustomerId, IEnumerable<CartItem> Items) : IRequest<Cart>;

    public class CreateCartHandler : IRequestHandler<CreateCartCommand, Cart>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public CreateCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork,
            IProductService productService)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public async Task<Cart> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            List<CartItem> cartItems = [];

            foreach (var item in request.Items)
            {
                var product = await _productService.Get(item.ProductId)
                    ?? throw new KeyNotFoundException($"Product: {item.ProductId} not found in catalog");

                cartItems.Add(new CartItem(product, item.Quantity));
            }

            Cart nCart = new(request.CustomerId, cartItems);

            await _cartRepository.AddAsync(nCart);
            await _unitOfWork.SaveChangesAsync();

            return nCart;
        }
    }
}