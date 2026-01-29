using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment environment,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _env = environment;
            _logger = logger;
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

                    await HandleExceptionDetails(ex, context);
                }
            }
        }

        private async Task HandleExceptionDetails(Exception ex, HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = httpContext.Response.StatusCode switch
                {
                    StatusCodes.Status400BadRequest => "Invalid request",
                    StatusCodes.Status401Unauthorized => "Unauthorized request",
                    StatusCodes.Status404NotFound => "Request not found",
                    _ => "Internal server error"
                },
                Detail = _env.IsDevelopment() ? ex.Message : "Oops.. something bad happened."
            };

            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}