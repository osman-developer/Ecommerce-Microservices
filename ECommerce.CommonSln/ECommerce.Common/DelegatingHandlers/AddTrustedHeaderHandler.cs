using Microsoft.Extensions.Configuration;

namespace ECommerce.Common.DelegatingHandlers
{
    public class AddTrustedHeaderHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        private readonly bool _useInternalApiKey;

        public AddTrustedHeaderHandler(IConfiguration configuration, bool useInternalApiKey = false)
        {
            _configuration = configuration;
            _useInternalApiKey = useInternalApiKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_useInternalApiKey)
            {
                var headerName = _configuration["InternalService:HeaderName"] ?? "X-Internal-ApiKey";
                var apiKey = _configuration["InternalService:ApiKey"] ?? string.Empty;

                if (!string.IsNullOrEmpty(apiKey))
                {
                    request.Headers.Remove(headerName);
                    request.Headers.Add(headerName, apiKey);
                }
            }
            else
            {
                var headerName = _configuration["ApiGateway:HeaderName"] ?? "Api-Gateway";
                var headerValue = _configuration["ApiGateway:HeaderValue"] ?? string.Empty;

                if (!string.IsNullOrEmpty(headerValue))
                {
                    request.Headers.Remove(headerName);
                    request.Headers.Add(headerName, headerValue);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
