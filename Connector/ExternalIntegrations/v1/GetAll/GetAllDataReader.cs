using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.ExternalIntegrations.v1.GetAll;

public class GetAllDataReader : TypedAsyncDataReaderBase<GetAllDataObject>
{
    private readonly ILogger<GetAllDataReader> _logger;
    private readonly ApiClient _apiClient;

    public GetAllDataReader(
        ILogger<GetAllDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<GetAllDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are null");
            yield break;
        }

        ApiResponse<IEnumerable<GetAllDataObject>>? response = null;
        try
        {
            response = await _apiClient.GetExternalIntegrations(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving external integrations");
            throw;
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve external integrations. Status code: {StatusCode}", response.StatusCode);
            yield break;
        }

        foreach (var integration in response.Data)
        {
            yield return integration;
        }
    }
}