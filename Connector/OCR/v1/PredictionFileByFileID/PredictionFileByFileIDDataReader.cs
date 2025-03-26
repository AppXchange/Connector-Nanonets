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

namespace Connector.OCR.v1.PredictionFileByFileID;

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

public class PredictionFileByFileIDDataReader : TypedAsyncDataReaderBase<PredictionFileByFileIDDataObject>
{
    private readonly ILogger<PredictionFileByFileIDDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public PredictionFileByFileIDDataReader(
        ILogger<PredictionFileByFileIDDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<PredictionFileByFileIDDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null || !dataObjectRunArguments.TryGetParameterValue("request_file_id", out string? requestFileId) || string.IsNullOrEmpty(requestFileId))
        {
            _logger.LogError("request_file_id parameter is required but was not provided");
            yield break;
        }

        PredictionFileByFileIDResponse? data;
        try
        {
            var response = await _apiClient.GetPredictionFileByFileId(
                modelId: _apiKeyAuth.ModelId,
                requestFileId: requestFileId,
                cancellationToken: cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to retrieve prediction file. API StatusCode: {StatusCode}", response.StatusCode);
                throw new Exception($"Failed to retrieve prediction file. API StatusCode: {response.StatusCode}");
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
            _logger.LogWarning("No prediction data returned from the API");
            yield break;
        }

        var results = new List<PredictionFileByFileIDDataObject>();
        if (data.ModeratedImagesCount > 0 && data.ModeratedImages != null)
        {
            results.AddRange(data.ModeratedImages);
        }

        if (data.UnmoderatedImagesCount > 0 && data.UnmoderatedImages != null)
        {
            results.AddRange(data.UnmoderatedImages);
        }

        foreach (var result in results)
        {
            yield return result;
        }
    }
}