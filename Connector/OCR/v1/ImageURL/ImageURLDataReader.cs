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
using Connector.OCR.v1.PredictionFileByFileID;
using AsyncPrediction = Connector.OCR.v1.PredictionFileByFileID;

namespace Connector.OCR.v1.ImageURL;

internal static class DataObjectExtensions
{
    public static bool TryGetParameterValue<T>(this DataObjectCacheWriteArguments args, string key, out T? value)
    {
        value = default;
        if (args == null) return false;

        var dict = args.GetType().GetProperty("Arguments")?.GetValue(args) as IDictionary<string, object>;
        if (dict == null || !dict.ContainsKey(key)) return false;

        try
        {
            value = (T)dict[key];
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class ImageURLDataReader : TypedAsyncDataReaderBase<ImageURLDataObject>
{
    private readonly ILogger<ImageURLDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public ImageURLDataReader(
        ILogger<ImageURLDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<ImageURLDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null)
        {
            _logger.LogError("Data object run arguments are required but were not provided");
            yield break;
        }

        var results = new List<ImageURLDataObject>();
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
                _logger.LogError("Failed to retrieve image URL data. API StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve image URL data. API StatusCode: {response.StatusCode}");
            }

            if (response.Data?.ModeratedImages != null)
            {
                results.AddRange(response.Data.ModeratedImages.Select<PredictionFileByFileIDDataObject, ImageURLDataObject>(MapToImageURLDataObject));
            }

            if (response.Data?.UnmoderatedImages != null)
            {
                results.AddRange(response.Data.UnmoderatedImages.Select<PredictionFileByFileIDDataObject, ImageURLDataObject>(MapToImageURLDataObject));
            }
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Exception while retrieving image URL data");
            throw;
        }

        foreach (var result in results)
        {
            yield return result;
        }
    }

    private static ImageURLDataObject MapToImageURLDataObject(PredictionFileByFileIDDataObject source)
    {
        return new ImageURLDataObject
        {
            Id = source.Id,
            Page = source.Page,
            RequestFileId = source.RequestFileId,
            FileUrl = source.FileUrl,
            ProcessingType = source.ProcessingType,
            Size = new AsyncPrediction.ImageSize 
            { 
                Width = source.Size?.Width ?? 0,
                Height = source.Size?.Height ?? 0
            }
        };
    }
}