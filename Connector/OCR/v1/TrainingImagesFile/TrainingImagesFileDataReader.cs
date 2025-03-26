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
using System.Threading.Tasks;

namespace Connector.OCR.v1.TrainingImagesFile;

public class TrainingImagesFileDataReader : TypedAsyncDataReaderBase<TrainingImagesFileDataObject>
{
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;
    private readonly ILogger<TrainingImagesFileDataReader> _logger;

    public TrainingImagesFileDataReader(
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth,
        ILogger<TrainingImagesFileDataReader> logger)
    {
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
        _logger = logger;
    }

    public override async IAsyncEnumerable<TrainingImagesFileDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            yield break;
        }

        ApiResponse<List<TrainingImagesFileDataObject>>? response = null;
        try
        {
            response = await _apiClient.GetTrainingImageFiles(_apiKeyAuth.ModelId, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving training image files");
            throw;
        }
        
        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve training image files. API StatusCode: {StatusCode}", response.StatusCode);
            yield break;
        }

        foreach (var item in response.Data)
        {
            yield return item;
        }
    }
}