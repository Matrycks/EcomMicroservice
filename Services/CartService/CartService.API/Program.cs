using System.Reflection;
using CartService.Application;
using CartService.Application.Interfaces;
using CartService.Infrastructure;
using CartService.Infrastructure.Data.Services;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("OrdersDb") ?? "OrdersDb",
    Environment.GetEnvironmentVariable("SERVICEBUS_NAMESPACE"), true)
    .AddHttpClient<IProductService, ProductService>(cfg =>
    {
        cfg.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Products:BaseAddress")!);
    });

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
