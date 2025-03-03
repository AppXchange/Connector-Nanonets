using System.Net.Http;
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
        request.Headers.Remove("X-Api-Key");
        request.Headers.Add("X-Api-Key", _apiKeyAuth.ApiKey);

        return await base.SendAsync(request, cancellationToken);
    }
}