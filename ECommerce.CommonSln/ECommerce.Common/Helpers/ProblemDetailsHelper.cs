using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace ECommerce.Common.Helpers
{
    public static class ProblemDetailsHelper
    {
        public static ProblemDetails CreateProblemDetails(int statusCode, string title, string instance, string? detail=null)
        {
            string finalDetail = string.IsNullOrEmpty(detail)
                                 ? $"A {statusCode} error occurred."
                                 : $"A {statusCode} error occurred. {detail}";
            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = finalDetail,
                Instance = instance
            };
        }
    }
}
