using System;
using System.Threading.Tasks;
using FitnessTracker.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace FitnessTracker.API.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip API key validation for JWT authenticated routes
            if (context.User.Identity?.IsAuthenticated == true)
            {
                await _next(context);
                return;
            }

            // Check for API key in header or query string
            if (!context.Request.Headers.TryGetValue("X-API-Key", out StringValues apiKeyHeaderValues) &&
                !context.Request.Query.TryGetValue("api_key", out StringValues apiKeyQueryValues))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            string apiKey = apiKeyHeaderValues.Count > 0 ? apiKeyHeaderValues[0] : apiKeyQueryValues[0];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            var apiKeyService = context.RequestServices.GetRequiredService<IApiKeyService>();
            var isValidApiKey = await apiKeyService.ValidateApiKeyAsync(apiKey);

            if (!isValidApiKey)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // Update last used timestamp
            await apiKeyService.UpdateApiKeyLastUsedAsync(apiKey);

            await _next(context);
        }
    }
}