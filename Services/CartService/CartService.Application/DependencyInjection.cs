using System.ComponentModel.Design;
using System.Reflection;
using CartService.Application.Carts;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<CreateCartHandler>();
        services.AddScoped<CartCheckoutHandler>();

        return services;
    }
}
