using Connector.Client;
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

namespace Connector.ImageClassification.v1.ImageFile.Predict;

public class PredictImageFileHandler : IActionHandler<PredictImageFileAction>
{
    private readonly ILogger<PredictImageFileHandler> _logger;
    private readonly ApiClient _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public PredictImageFileHandler(
        ILogger<PredictImageFileHandler> logger,
        ApiClient apiClient,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _apiClient = apiClient;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ActionHandlerOutcome> HandleQueuedActionAsync(ActionInstance actionInstance, CancellationToken cancellationToken)
    {
        var input = JsonSerializer.Deserialize<PredictImageFileActionInput>(actionInstance.InputJson);
        try
        {
            using var fileClient = _httpClientFactory.CreateClient();
            using var fileResponse = await fileClient.GetAsync(
                input.File,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken: cancellationToken);

            if (!fileResponse.IsSuccessStatusCode)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = fileResponse.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(PredictImageFileHandler) },
                            Text = $"Failed to retrieve file from URL: {input.File}"
                        }
                    }
                });
            }

            var response = await _apiClient.PredictImageFile(
                await fileResponse.Content.ReadAsStreamAsync(cancellationToken),
                cancellationToken);

            if (!response.IsSuccessful || response.Data == default)
            {
                return ActionHandlerOutcome.Failed(new StandardActionFailure
                {
                    Code = response.StatusCode.ToString(),
                    Errors = new[]
                    {
                        new Error
                        {
                            Source = new[] { nameof(PredictImageFileHandler) },
                            Text = $"Failed to predict image file. Status code: {response.StatusCode}"
                        }
                    }
                });
            }

            var operations = new List<SyncOperation>();
            var keyResolver = new DefaultDataObjectKey();
            var key = keyResolver.BuildKeyResolver()(response.Data);
            operations.Add(SyncOperation.CreateSyncOperation(UpdateOperation.Upsert.ToString(), key.UrlPart, key.PropertyNames, response.Data));

            var resultList = new List<CacheSyncCollection>
            {
                new CacheSyncCollection() { DataObjectType = typeof(ImageFileDataObject), CacheChanges = operations.ToArray() }
            };

            return ActionHandlerOutcome.Successful(response.Data, resultList);
        }
        catch (HttpRequestException exception)
        {
            var errorSource = new List<string> { nameof(PredictImageFileHandler) };
            if (string.IsNullOrEmpty(exception.Source)) errorSource.Add(exception.Source!);
            
            return ActionHandlerOutcome.Failed(new StandardActionFailure
            {
                Code = exception.StatusCode?.ToString() ?? "500",
                Errors = new[]
                {
                    new Error
                    {
                        Source = errorSource.ToArray(),
                        Text = exception.Message
                    }
                }
            });
        }
    }
}
