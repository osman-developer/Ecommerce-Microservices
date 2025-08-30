using ECommerce.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerce.Common.Middleware
{
    public static class StatusCodeMiddleware
    {
        // Triggered when a specific status code (like 404 or 401) is returned by the pipeline.
        public static void UseCustomStatusCodePages(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("StatusCodeMiddleware");

            app.UseStatusCodePages(async context =>
            {
                var httpContext = context.HttpContext;
                var response = httpContext.Response;
                var statusCode = response.StatusCode;


                string title = statusCode switch
                {
                    StatusCodes.Status401Unauthorized => "Unauthorized",
                    StatusCodes.Status403Forbidden => "Forbidden",
                    StatusCodes.Status404NotFound => "Not Found",
                    StatusCodes.Status429TooManyRequests => "Too Many Requests",
                    _ => "Error"
                };

                logger.LogWarning("Returning {StatusCode} for {Path}", statusCode, httpContext.Request.Path);

                // Create ProblemDetails using a helper method
                var problemDetails = ProblemDetailsHelper.CreateProblemDetails(statusCode, title, httpContext.Request.Path);

                response.ContentType = "application/json";
                await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            });
        }

    }
}
