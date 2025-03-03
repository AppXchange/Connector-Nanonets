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

namespace Connector.OCR.v1.PredictionFileByFileID;

public class PredictionFileByFileIDDataReader : TypedAsyncDataReaderBase<PredictionFileByFileIDDataObject>
{
    private readonly ILogger<PredictionFileByFileIDDataReader> _logger;
    private int _currentPage = 0;

    public PredictionFileByFileIDDataReader(
        ILogger<PredictionFileByFileIDDataReader> logger)
    {
        _logger = logger;
    }

    public override async IAsyncEnumerable<PredictionFileByFileIDDataObject> GetTypedDataAsync(DataObjectCacheWriteArguments ? dataObjectRunArguments, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            var response = new ApiResponse<PaginatedResponse<PredictionFileByFileIDDataObject>>();
            // If the PredictionFileByFileIDDataObject does not have the same structure as the PredictionFileByFileID response from the API, create a new class for it and replace PredictionFileByFileIDDataObject with it.
            // Example:
            // var response = new ApiResponse<IEnumerable<PredictionFileByFileIDResponse>>();

            // Make a call to your API/system to retrieve the objects/type for the connector's configuration.
            try
            {
                //response = await _apiClient.GetRecords<PredictionFileByFileIDDataObject>(
                //    relativeUrl: "predictionFileByFileIDs",
                //    page: _currentPage,
                //    cancellationToken: cancellationToken)
                //    .ConfigureAwait(false);
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(exception, "Exception while making a read request to data object 'PredictionFileByFileIDDataObject'");
                throw;
            }

            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to retrieve records for 'PredictionFileByFileIDDataObject'. API StatusCode: {response.StatusCode}");
            }

            if (response.Data == null || !response.Data.Items.Any()) break;

            // Return the data objects to Cache.
            foreach (var item in response.Data.Items)
            {
                // If new class was created to match the API response, create a new PredictionFileByFileIDDataObject object, map the properties and return a PredictionFileByFileIDDataObject.

                // Example:
                //var resource = new PredictionFileByFileIDDataObject
                //{
                //// TODO: Map properties.      
                //};
                //yield return resource;
                yield return item;
            }

            // Handle pagination per API client design
            _currentPage++;
            if (_currentPage >= response.Data.TotalPages)
            {
                break;
            }
        }
    }
}