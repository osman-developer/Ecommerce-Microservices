namespace ECommerce.ApiGateway.API.DelegatingHandlers
{
    public class AddApiGatewayHeaderHandler : DelegatingHandler
    {
        private readonly string _headerName;
        private readonly string _headerValue;

        public AddApiGatewayHeaderHandler(IConfiguration configuration)
        {
            _headerName = configuration["ApiGateway:HeaderName"] ?? "Api-Gateway";
            _headerValue = configuration["ApiGateway:HeaderValue"] ?? string.Empty;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Add the header to every outgoing request
            if (!_headerValue.Equals(string.Empty))
            {
                request.Headers.Remove(_headerName);
                request.Headers.Add(_headerName, _headerValue);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
