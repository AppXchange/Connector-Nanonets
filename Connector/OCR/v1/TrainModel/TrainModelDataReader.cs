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
using Connector.OCR.v1.TrainModel.Train;

namespace Connector.OCR.v1.TrainModel;

public class TrainModelDataReader : TypedAsyncDataReaderBase<TrainModelDataObject>
{
    private readonly ILogger<TrainModelDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public TrainModelDataReader(
        ILogger<TrainModelDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<TrainModelDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            yield break;
        }

        ApiResponse<TrainTrainModelActionOutput>? response = null;
        try
        {
            response = await _apiClient.TrainModel(
                modelId: _apiKeyAuth.ModelId,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful || response.Data == null)
            {
                _logger.LogError("Failed to train model. API StatusCode: {StatusCode}", response.StatusCode);
                yield break;
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while training model");
            throw;
        }

        if (response?.Data != null)
        {
            yield return new TrainModelDataObject
            {
                ModelId = response.Data.ModelId ?? string.Empty,
                ModelType = response.Data.ModelType,
                State = response.Data.State,
                Categories = response.Data.Categories?.Select(c => new Category 
                { 
                    Name = c.Name,
                    Count = c.Count 
                }).ToList()
            };
        }
    }
}