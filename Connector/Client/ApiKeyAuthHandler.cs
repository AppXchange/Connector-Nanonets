using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.AuthTypes;

namespace Connector.Client;

public class ApiKeyAuthHandler : DelegatingHandler
{
    private readonly IApiKeyAuth _apiKeyAuth;

    public ApiKeyAuthHandler(IApiKeyAuth apiKeyAuth)
    {
        _apiKeyAuth = apiKeyAuth;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Add Basic Authentication header with API key as username and empty password
        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKeyAuth.ApiKey}:"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        return await base.SendAsync(request, cancellationToken);
    }
}