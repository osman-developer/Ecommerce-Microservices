using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace ECommerce.Common.Middleware
{
    public static class SharedMiddlewareExtensions
    {
        public static IApplicationBuilder UseSharedMiddlewares(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseAllowOnlyApiGateway();
            app.UseGlobalExceptionHandler();
            app.UseCustomStatusCodePages(loggerFactory);
            return app;
        }
    }
}