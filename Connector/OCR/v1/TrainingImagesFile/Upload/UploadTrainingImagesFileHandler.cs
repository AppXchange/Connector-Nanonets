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

namespace Connector.OCR.v1.TrainingImagesFile.Upload;

public class UploadTrainingImagesFileHandler : IActionHandler<UploadTrainingImagesFileAction>
{
    private readonly ILogger<UploadTrainingImagesFileHandler> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public UploadTrainingImagesFileHandler(
        ILogger<UploadTrainingImagesFileHandler> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<UploadTrainingImagesFileActionInput>(actionInstance.InputJson);
        if (input == null)
        {
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = "400",
                Errors = new[]
                {
                    new Xchange.Connector.SDK.Action.Error
                    {
                        Source = new[] { "UploadTrainingImagesFileHandler" },
                        Text = "Invalid input data"
                    }
                }
            });
        }

        try
        {
            var response = await _apiClient.PostTrainingImageFile(
                modelId: _apiKeyAuth.ModelId,
                file: input.File,
                base64Data: input.Base64Data,
                requestMetadata: input.RequestMetadata,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to upload training image file. API StatusCode: {StatusCode}", response.StatusCode);
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Xchange.Connector.SDK.Action.Error
                        {
                            Source = new[] { "UploadTrainingImagesFileHandler" },
                            Text = $"Failed to upload training image file: {response.StatusCode}"
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
                new CacheSyncCollection() { DataObjectType = typeof(UploadResult), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while uploading training image file");
            var errorSource = new List<string> { "UploadTrainingImagesFileHandler" };
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
