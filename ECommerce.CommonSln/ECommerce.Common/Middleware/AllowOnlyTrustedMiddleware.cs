using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Common.Middleware
{
    public class AllowOnlyTrustedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _gatewayHeaderName;
        private readonly string _gatewayHeaderValue;
        private readonly string _internalApiKeyHeaderName;
        private readonly string _internalApiKey;

        public AllowOnlyTrustedMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            _gatewayHeaderName = configuration["ApiGateway:HeaderName"] ?? "Api-Gateway";
            _gatewayHeaderValue = configuration["ApiGateway:HeaderValue"] ?? string.Empty;

            _internalApiKeyHeaderName = configuration["InternalService:HeaderName"] ?? "X-Internal-ApiKey";
            _internalApiKey = configuration["InternalService:ApiKey"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isGatewayRequest = context.Request.Headers.TryGetValue(_gatewayHeaderName, out var gatewayHeaderValue) &&
                                   (_gatewayHeaderValue == string.Empty || gatewayHeaderValue == _gatewayHeaderValue);

            var isInternalRequest = context.Request.Headers.TryGetValue(_internalApiKeyHeaderName, out var apiKeyHeaderValue) &&
                                    apiKeyHeaderValue == _internalApiKey;

            if (!isGatewayRequest && !isInternalRequest)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(
                    "{\"error\": \"Access denied. Requests must come through the API Gateway or be from a trusted service.\"}",
                    context.RequestAborted
                );
                return;
            }

            await _next(context);
        }
    }

    public static class AllowOnlyTrustedMiddlewareExtensions
    {
        public static IApplicationBuilder UseAllowOnlyTrusted(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AllowOnlyTrustedMiddleware>();
        }
    }
}
