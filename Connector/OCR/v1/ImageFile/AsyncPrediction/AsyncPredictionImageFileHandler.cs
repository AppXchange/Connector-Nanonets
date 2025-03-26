using Connector.Client;
using Connector.Connections;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;

namespace Connector.OCR.v1.ImageFile.AsyncPrediction;

public class AsyncPredictionImageFileHandler : IActionHandler<AsyncPredictionImageFileAction>
{
    private readonly ILogger<AsyncPredictionImageFileHandler> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public AsyncPredictionImageFileHandler(
        ILogger<AsyncPredictionImageFileHandler> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<AsyncPredictionImageFileActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "AsyncPredictionImageFileHandler" },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.PostImageFileAsync(
                modelId: _apiKeyAuth.ModelId,
                file: input.File,
                base64Data: input.Base64Data,
                requestMetadata: input.RequestMetadata,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to submit image for async prediction. API StatusCode: {StatusCode}", response.StatusCode);
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = new[] { "AsyncPredictionImageFileHandler" },
                            Text = $"Failed to submit image for async prediction: {response.StatusCode}"
                        }
                    }
                });
            }

            var operations = new List<SyncOperation>();
            if (response.Data?.Result != null)
            {
                foreach (var result in response.Data.Result)
                {
                    var keyResolver = new DefaultDataObjectKey();
                    var key = keyResolver.BuildKeyResolver()(result);
                    operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, result));
                }
            }

            var resultList = new List<CacheSyncCollection>
            {
                new CacheSyncCollection() { DataObjectType = typeof(ImageFileDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while submitting image for async prediction");
            var errorSource = new List<string> { "AsyncPredictionImageFileHandler" };
            if (!string.IsNullOrEmpty(exception.Source)) errorSource.Add(exception.Source);
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = errorSource.ToArray(),
                        Text = exception.Message
                    }
                }
            });
        }
    }
}
