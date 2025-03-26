using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Connector.Client;
using Connector.Connections;
using Microsoft.Extensions.Logging;
using Xchange.Connector.SDK.Action;
using Xchange.Connector.SDK.CacheWriter;
using ESR.Hosting.Action;
using ESR.Hosting.CacheWriter;
using Xchange.Connector.SDK.Client.AppNetwork;

namespace Connector.OCR.v1.TrainingImagesURL.Upload;

public class UploadTrainingImagesURLHandler : IActionHandler<UploadTrainingImagesURLAction>
{
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;
    private readonly ILogger<UploadTrainingImagesURLHandler> _logger;

    public UploadTrainingImagesURLHandler(
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth,
        ILogger<UploadTrainingImagesURLHandler> logger)
    {
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
        _logger = logger;
    }

    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UploadTrainingImagesURLActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error { Text = "Invalid input data" } }
            });
        }

        try
        {
            var response = await _apiClient.PostTrainingImageUrls(
                modelId: _apiKeyAuth.ModelId,
                urls: input.Urls,
                requestMetadata: input.RequestMetadata,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful || response.Data == null)
            {
                _logger.LogError("Failed to upload training image URLs. API StatusCode: {StatusCode}", response.StatusCode);
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[] { new Xchange.Connector.SDK.Action.Error { Text = $"Failed to upload training image URLs: {response.StatusCode}" } }
                });
            }

            var syncOperations = new List<SyncOperation>();
            foreach (var result in response.Data.Result ?? new List<UploadResult>())
            {
                if (result.Id != null)
                {
                    syncOperations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), "id", new[] { "id" }, result));
                }
            }

            return ActionHandlerOutcome.Successful(new CacheSyncCollection
            {
                DataObjectType = typeof(UploadResult),
                CacheChanges = syncOperations.ToArray()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading training image URLs");
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "500",
                Errors = new[] { new Xchange.Connector.SDK.Action.Error { Text = ex.Message } }
            });
        }
    }
}
