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

namespace Connector.OCR.v1.PredictionFileByPageID;

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

public class PredictionFileByPageIDDataReader : TypedAsyncDataReaderBase<PredictionFileByPageIDDataObject>
{
    private readonly ILogger<PredictionFileByPageIDDataReader> _logger;
    private readonly ApiClient _apiClient;
    private readonly INanonetsApiKeyAuth _apiKeyAuth;

    public PredictionFileByPageIDDataReader(
        ILogger<PredictionFileByPageIDDataReader> logger,
        ApiClient apiClient,
        INanonetsApiKeyAuth apiKeyAuth)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiKeyAuth = apiKeyAuth;
    }

    public override async IAsyncEnumerable<PredictionFileByPageIDDataObject> GetTypedDataAsync(
        DataObjectCacheWriteArguments? dataObjectRunArguments,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (dataObjectRunArguments == null || !dataObjectRunArguments.TryGetParameterValue("page_id", out string? pageId) || string.IsNullOrEmpty(pageId))
        {
            _logger.LogError("page_id parameter is required but was not provided");
            yield break;
        }

        PredictionFileByPageIDResponse? data;
        try
        {
            var response = await _apiClient.GetPredictionFileByPageId(
                modelId: _apiKeyAuth.ModelId,
                pageId: pageId,
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

        if (data?.Result == null || !data.Result.Any())
        {
            _logger.LogWarning("No prediction data returned from the API");
            yield break;
        }

        foreach (var result in data.Result)
        {
            yield return result;
        }
    }
}