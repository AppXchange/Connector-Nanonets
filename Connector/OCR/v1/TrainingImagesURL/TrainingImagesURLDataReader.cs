using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Connector.Client;
using Connector.Connections;
using Microsoft.Extensions.Logging;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;
using System.Runtime.CompilerServices;
using ESR.Hosting.CacheWriter;

namespace Connector.OCR.v1.TrainingImagesURL;

public class TrainingImagesURLDataReader : TypedAsyncDataReaderBase<TrainingImagesURLDataObject>
{
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;
    private readonly ILogger<TrainingImagesURLDataReader> _logger;

    public TrainingImagesURLDataReader(
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth,
        ILogger<TrainingImagesURLDataReader> logger)
    {
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
        _logger = logger;
    }

    public override async IAsyncEnumerable<TrainingImagesURLDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments is null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            yield break;
        }

        ApiResponse<List<TrainingImagesURLDataObject>>? response = null;
        try
        {
            response = await _apiClient.GetTrainingImageUrls(_apiKeyAuth.ModelId, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving training image URLs");
            throw;
        }

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Failed to retrieve training image URLs. API StatusCode: {StatusCode}", response.StatusCode);
            yield break;
        }

        foreach (var item in response.Data)
        {
            yield return item;
        }
    }
}