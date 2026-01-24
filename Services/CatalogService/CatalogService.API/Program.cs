using CatalogService.API.Middleware;
using CatalogService.Application;
using CatalogService.Infrastructure;
using CatalogService.Infrastructure.DataInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("CatalogDb") ?? "CatalogDb", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

ProductsInitializer.Seed(app.Services);

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
