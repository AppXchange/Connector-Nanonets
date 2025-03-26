using System;
using Xchange.Connector.SDK.Client.AuthTypes;
using Xchange.Connector.SDK.Client.ConnectionDefinitions.Attributes;

namespace Connector.Connections;

public interface INanonetsApiKeyAuth : IApiKeyAuth
{
    string ModelId { get; }
}

[ConnectionDefinition(title: "ApiKeyAuth", description: "Authentication for Nanonets API using API Key")]
public class ApiKeyAuth : INanonetsApiKeyAuth
{
    [ConnectionProperty(
        title: "API Key", 
        description: "Your Nanonets API key. Can be found in Account Info -> API Keys section of the Nanonets dashboard.", 
        isRequired: true, 
        isSensitive: true)]
    public string ApiKey { get; init; } = string.Empty;

    [ConnectionProperty(
        title: "Connection Environment", 
        description: "The environment to connect to (Production or Test). Both environments use the same base URL for Nanonets.", 
        isRequired: true, 
        isSensitive: false)]
    public ConnectionEnvironmentApiKeyAuth ConnectionEnvironment { get; set; } = ConnectionEnvironmentApiKeyAuth.Unknown;

    [ConnectionProperty(
        title: "Model ID", 
        description: "The Nanonets model ID to use for predictions.", 
        isRequired: true, 
        isSensitive: false)]
    public string ModelId { get; init; } = string.Empty;

    public string BaseUrl
    {
        get
        {
            switch (ConnectionEnvironment)
            {
                case ConnectionEnvironmentApiKeyAuth.Production:
                    return "https://app.nanonets.com";
                case ConnectionEnvironmentApiKeyAuth.Test:
                    return "https://app.nanonets.com";
                default:
                    throw new Exception("No base url was set.");
            }
        }
    }
}

public enum ConnectionEnvironmentApiKeyAuth
{
    Unknown = 0,
    Production = 1,
    Test = 2
}