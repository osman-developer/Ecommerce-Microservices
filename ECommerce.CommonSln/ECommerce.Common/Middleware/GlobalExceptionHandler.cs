using ECommerce.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ECommerce.Common.Middleware
{
    public class GlobalExceptionHandler
    {
        //is triggered when a specific Exception happens internally(like bug,code 500,TimeOut,TaskCanceled).
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Custom logging for specific exceptions and creating ProblemDetails
                ProblemDetails problemDetails;

                switch (ex)
                {
                    case TaskCanceledException taskEx:
                        _logger.LogWarning(taskEx, "Request was canceled due to a timeout. Path: {Path}, Message: {Message}", context.Request.Path, ex.Message);
                        problemDetails = ProblemDetailsHelper.CreateProblemDetails((int)HttpStatusCode.RequestTimeout, "Request Timeout", context.Request.Path, "The request took too long to process. Please try again later.");
                        break;

                    case TimeoutException timeoutEx:
                        _logger.LogWarning(timeoutEx, "Request timed out. Path: {Path}, Message: {Message}", context.Request.Path, ex.Message);
                        problemDetails = ProblemDetailsHelper.CreateProblemDetails((int)HttpStatusCode.RequestTimeout, "Request Timeout", context.Request.Path, "The request has timed out. Please try again later.");
                        break;

                    default:
                        _logger.LogError(ex, "Unhandled exception occurred. Path: {Path}, Message: {Message}", context.Request.Path, ex.Message);
                        problemDetails = ProblemDetailsHelper.CreateProblemDetails((int)HttpStatusCode.InternalServerError, "Internal Server Error", context.Request.Path, "An unexpected error occurred. Please try again later.");
                        break;
                }

                // Set response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = problemDetails.Status.Value;
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            }
        }

    }

    // Extension method for clean registration
    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
