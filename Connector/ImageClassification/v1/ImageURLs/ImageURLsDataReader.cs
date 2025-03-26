using Connector.Client;
using Connector.Connections;
using System;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;

namespace Connector.ImageClassification.v1.ImageURLs;

public class ImageURLsDataReader : TypedAsyncDataReaderBase<ImageURLsDataObject>
{
    private readonly ILogger<ImageURLsDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public ImageURLsDataReader(
        ILogger<ImageURLsDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override IAsyncEnumerable<ImageURLsDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            return AsyncEnumerable.Empty<ImageURLsDataObject>();
        }

        _logger.LogInformation("Image classification data reader is not supported as the model status endpoint is not available");
        return AsyncEnumerable.Empty<ImageURLsDataObject>();
    }
}