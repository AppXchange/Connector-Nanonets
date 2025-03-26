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
using Connector.Connections;
using Connector.OCR.v1.PredictionFileByFileID;

namespace Connector.OCR.v1.ImageFile;

public class ImageFileDataReader : TypedAsyncDataReaderBase<ImageFileDataObject>
{
    private readonly ILogger<ImageFileDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public ImageFileDataReader(
        ILogger<ImageFileDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<ImageFileDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            yield break;
        }

        var results = new List<ImageFileDataObject>();
        try
        {
            if (!dataObjectRunArguments.TryGetParameterValue("request_file_id", out string? requestFileId) || string.IsNullOrEmpty(requestFileId))
            {
                _logger.LogError("request_file_id parameter is required but was not provided");
                yield break;
            }

            var response = await _apiClient.GetPredictionFileByFileId(
                modelId: _apiKeyAuth.ModelId,
                requestFileId: requestFileId,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve image file data. API StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve image file data. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.ModeratedImages != null)
            {
                results.AddRange(response.Data.ModeratedImages.Select(MapToImageFileDataObject));
            }

            if (response.Data?.UnmoderatedImages != null)
            {
                results.AddRange(response.Data.UnmoderatedImages.Select(MapToImageFileDataObject));
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving image file data");
            throw;
        }

        foreach (var result in results)
        {
            yield return result;
        }
    }

    private static ImageFileDataObject MapToImageFileDataObject(PredictionFileByFileIDDataObject source)
    {
        return new ImageFileDataObject
        {
            Id = source.Id,
            Page = source.Page,
            RequestFileId = source.RequestFileId,
            FileUrl = source.FileUrl,
            ProcessingType = source.ProcessingType,
            Size = new ImageSize 
            { 
                Width = source.Size?.Width ?? 0,
                Height = source.Size?.Height ?? 0
            }
        };
    }
}