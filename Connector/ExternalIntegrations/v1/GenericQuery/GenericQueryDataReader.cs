using Connector.Client;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.ExternalIntegrations.v1.GenericQuery;

public class GenericQueryDataReader : TypedAsyncDataReaderBase<GenericQueryDataObject>
{
    private readonly ILogger<GenericQueryDataReader> _logger;
    private readonly ApiClient _apiClient;

    public GenericQueryDataReader(
        ILogger<GenericQueryDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override IAsyncEnumerable<GenericQueryDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            return AsyncEnumerable.Empty<GenericQueryDataObject>();
        }

        _logger.LogInformation("Generic query data reader is not supported as it requires a specific query to be executed");
        return AsyncEnumerable.Empty<GenericQueryDataObject>();
    }
}