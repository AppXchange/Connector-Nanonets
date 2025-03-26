using Connector.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Client.Testing;

namespace Connector.Connections
{
    public class ConnectionTestHandler : IConnectionTestHandler
    {
        private readonly ILogger<IConnectionTestHandler> _logger;
        private readonly ApiClient _apiClient;

        public ConnectionTestHandler(ILogger<IConnectionTestHandler> logger, ApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task<TestConnectionResult> TestConnection()
        {
            try
            {
                var response = await _apiClient.TestConnection();

                if (response == null)
                {
                    return new TestConnectionResult()
                    {
                        Success = false,
                        Message = "Failed to get response from server",
                        StatusCode = 500
                    };
                }

                if (response.IsSuccessful)
                {
                    return new TestConnectionResult()
                    {
                        Success = true,
                        Message = "Successfully authenticated with Nanonets API",
                        StatusCode = response.StatusCode
                    };
                }

                switch (response.StatusCode)
                {
                    case 401:
                        return new TestConnectionResult()
                        {
                            Success = false,
                            Message = "Authentication failed. Please check your API key.",
                            StatusCode = response.StatusCode
                        };
                    case 403:
                        return new TestConnectionResult()
                        {
                            Success = false,
                            Message = "Access forbidden. Your API key may not have the required permissions.",
                            StatusCode = response.StatusCode
                        };
                    default:
                        return new TestConnectionResult()
                        {
                            Success = false,
                            Message = $"Connection test failed with status code {response.StatusCode}",
                            StatusCode = response.StatusCode
                        };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing connection to Nanonets API");
                return new TestConnectionResult()
                {
                    Success = false,
                    Message = $"Error testing connection: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
