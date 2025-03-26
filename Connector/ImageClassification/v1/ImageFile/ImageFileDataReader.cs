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

namespace Connector.ImageClassification.v1.ImageFile;

public class ImageFileDataReader : TypedAsyncDataReaderBase<ImageFileDataObject>
{
    private readonly ILogger<ImageFileDataReader> _logger;
    private readonly ApiClient _apiClient;

    public ImageFileDataReader(
        ILogger<ImageFileDataReader> logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public override async IAsyncEnumerable<ImageFileDataObject> GetTypedDataAsync(DataObjectCacheWriteArguments? dataObjectRunArguments, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("dataObjectRunArguments is null");
            yield break;
        }

        _logger.LogInformation("Image classification data reader is not supported as the model status endpoint is not available");
        yield break;
    }
}