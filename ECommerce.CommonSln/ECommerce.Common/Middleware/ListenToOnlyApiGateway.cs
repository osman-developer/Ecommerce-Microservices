using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Common.Middleware
{
    public class AllowOnlyApiGatewayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _gatewayHeaderName;
        private readonly string _gatewayHeaderValue;

        public AllowOnlyApiGatewayMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _gatewayHeaderName = configuration["ApiGateway:HeaderName"] ?? "Api-Gateway";
            _gatewayHeaderValue = configuration["ApiGateway:HeaderValue"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_gatewayHeaderName, out var headerValue) ||
                (_gatewayHeaderValue != string.Empty && headerValue != _gatewayHeaderValue))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(
                    "{\"error\": \"Access denied. Requests must come through the API Gateway.\"}",
                    context.RequestAborted
                );
                return;
            }

            await _next(context);
        }
    }

    public static class AllowOnlyApiGatewayMiddlewareExtensions
    {
        public static IApplicationBuilder UseAllowOnlyApiGateway(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AllowOnlyApiGatewayMiddleware>();
        }
    }
}
