using Connector.Client;
using Connector.Connections;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xchange.Connector.SDK.CacheWriter;
using System.Net.Http;
using Xchange.Connector.SDK.Client.AuthTypes;

namespace Connector.OCR.v1.PredictionFiles;

public class PredictionFilesDataReader : TypedAsyncDataReaderBase<PredictionFilesDataObject>
{
    private readonly ILogger<PredictionFilesDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;
    private int _currentPage = 0;

    public PredictionFilesDataReader(
        ILogger<PredictionFilesDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<PredictionFilesDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments, 
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var startDayInterval = (int)(DateTime.UtcNow.Date.AddDays(-30).Subtract(new DateTime(1970, 1, 1))).TotalDays;
        var currentBatchDay = (int)(DateTime.UtcNow.Date.Subtract(new DateTime(1970, 1, 1))).TotalDays;

        PredictionFilesResponse? data;
        try
        {
            var response = await _apiClient.GetPredictionFiles(
                modelId: _apiKeyAuth.ModelId,
                startDayInterval: startDayInterval,
                currentBatchDay: currentBatchDay,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve prediction files. API StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve prediction files. API StatusCode: {response.StatusCode}");
            }

            data = response.Data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Exception while making a read request to Nanonets API");
            throw;
        }

        if (data == null)
        {
            _logger.LogWarning("No prediction files data returned from the API");
            yield break;
        }

        foreach (var image in data.ModeratedImages ?? Enumerable.Empty<PredictionFilesDataObject>())
        {
            yield return image;
        }

        foreach (var image in data.UnmoderatedImages ?? Enumerable.Empty<PredictionFilesDataObject>())
        {
            yield return image;
        }
    }
}