using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = context.TraceIdentifier;
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                try
                {
                    string reqPath = context.Request.Path;

                    _logger.LogInformation("Endpoint requested: {reqPath}", reqPath);

                    await _next(context);

                    _logger.LogInformation("Endpoint succeeded: {reqPath}", reqPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception");

                    await HandleExceptionDetails(context, ex);
                }
            }
        }

        private async Task HandleExceptionDetails(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = context.Response.StatusCode switch
                {
                    StatusCodes.Status400BadRequest => "Invalid request",
                    StatusCodes.Status401Unauthorized => "Unauthorized request",
                    StatusCodes.Status404NotFound => "Request not found",
                    _ => "Internal server error"
                },
                Detail = _env.IsDevelopment() ? ex.Message : "Oops.. something bad happened."
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}