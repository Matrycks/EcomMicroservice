using System.Reflection;
using CartService.Application;
using CartService.Application.Interfaces;
using CartService.Infrastructure;
using CartService.Infrastructure.Data.Services;
using Microsoft.VisualBasic;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Cart API",
        Version = "v1",
        Description = "Ecommerce Cart API"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("OrdersDb") ?? "OrdersDb",
    Environment.GetEnvironmentVariable("SERVICEBUS_NAMESPACE"), true)
    .AddHttpClient<IProductService, ProductService>(cfg =>
    {
        cfg.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Products:BaseAddress")!);
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
});

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
