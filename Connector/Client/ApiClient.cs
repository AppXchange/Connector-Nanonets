using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.IO;
using Connector.OCR.v1.PredictionFiles;
using System.Net.Http.Headers;
using Connector.OCR.v1.ImageFile.AsyncPrediction;
using Connector.OCR.v1.ImageFile.Prediction;
using Connector.OCR.v1.ImageURL.AsyncPrediction;
using Connector.OCR.v1.ImageURL.Prediction;
using System.Text.Json;
using Connector.OCR.v1.TrainingImagesFile.Upload;
using Connector.OCR.v1.TrainingImagesFile;
using Connector.OCR.v1.TrainingImagesURL.Upload;
using Connector.OCR.v1.TrainingImagesURL;
using Connector.OCR.v1.TrainModel.Train;
using Connector.OCR.v1.TrainModel;
using Connector.ImageClassification.v1.ImageFile.Predict;
using Connector.ImageClassification.v1.ImageFile;
using Connector.ImageClassification.v1.ImageURLs.Predict;
using Connector.ExternalIntegrations.v1.GetAll;
using Connector.ExternalIntegrations.v1.GenericQuery.Execute;

namespace Connector.Client;

/// <summary>
/// A client for interfacing with the API via the HTTP protocol.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    // Example of a paginated response.
    public async Task<ApiResponse<PaginatedResponse<T>>> GetRecords<T>(string relativeUrl, int page, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{relativeUrl}?page={page}", cancellationToken: cancellationToken).ConfigureAwait(false);
        return new ApiResponse<PaginatedResponse<T>>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<PaginatedResponse<T>>(cancellationToken: cancellationToken) : default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> GetNoContent(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("no-content", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse> TestConnection(CancellationToken cancellationToken = default)
    {
        // Test connection by calling the v2 API endpoint
        var response = await _httpClient
            .GetAsync("v2/", cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<PredictionFilesResponse>> GetPredictionFiles(
        string modelId,
        int startDayInterval,
        int currentBatchDay,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"api/v2/Inferences/Model/{modelId}/ImageLevelInferences?start_day_interval={startDayInterval}&current_batch_day={currentBatchDay}", 
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<PredictionFilesResponse>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<PredictionFilesResponse>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<PredictionFileByPageIDResponse>> GetPredictionFileByPageId(
        string modelId,
        string pageId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"api/v2/Inferences/Model/{modelId}/ImageLevelInferences/{pageId}", 
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<PredictionFileByPageIDResponse>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<PredictionFileByPageIDResponse>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<PredictionFileByFileIDResponse>> GetPredictionFileByFileId(
        string modelId,
        string requestFileId,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync($"api/v2/Inferences/Model/{modelId}/InferenceRequestFiles/GetPredictions/{requestFileId}", 
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse<PredictionFileByFileIDResponse>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? 
                await response.Content.ReadFromJsonAsync<PredictionFileByFileIDResponse>(cancellationToken: cancellationToken) : 
                default,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken: cancellationToken)
        };
    }

    public async Task<ApiResponse<AsyncPredictionImageFileActionOutput>> PostImageFileAsync(
        string modelId,
        string file,
        string? base64Data = null,
        string? requestMetadata = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/OCR/Model/{modelId}/LabelFile?async=true");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(file), "file");
        if (!string.IsNullOrEmpty(base64Data))
        {
            content.Add(new StringContent(base64Data), "base64_data");
        }
        if (!string.IsNullOrEmpty(requestMetadata))
        {
            content.Add(new StringContent(requestMetadata), "request_metadata");
        }
        request.Content = content;

        return await SendRequestAsync<AsyncPredictionImageFileActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<PredictionImageFileActionOutput>> PostImageFile(
        string modelId,
        string file,
        string? base64Data = null,
        string? requestMetadata = null,
        string? pagesToProcess = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/OCR/Model/{modelId}/LabelFile/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(file), "file");
        if (!string.IsNullOrEmpty(base64Data))
        {
            content.Add(new StringContent(base64Data), "base64_data");
        }
        if (!string.IsNullOrEmpty(requestMetadata))
        {
            content.Add(new StringContent(requestMetadata), "request_metadata");
        }
        if (!string.IsNullOrEmpty(pagesToProcess))
        {
            content.Add(new StringContent(pagesToProcess), "pages_to_process");
        }
        request.Content = content;

        return await SendRequestAsync<PredictionImageFileActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<PredictionImageURLActionOutput>> PostImageURL(
        string modelId,
        List<string> urls,
        string? base64Data = null,
        string? requestMetadata = null,
        string? pagesToProcess = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/OCR/Model/{modelId}/LabelUrls/");
        request.Headers.Add("accept", "application/json");

        var content = new MultipartFormDataContent();
        foreach (var url in urls)
        {
            content.Add(new StringContent(url), "urls");
        }

        if (!string.IsNullOrEmpty(base64Data))
        {
            content.Add(new StringContent(base64Data), "base64_data");
        }

        if (!string.IsNullOrEmpty(requestMetadata))
        {
            content.Add(new StringContent(requestMetadata), "request_metadata");
        }

        if (!string.IsNullOrEmpty(pagesToProcess))
        {
            content.Add(new StringContent(pagesToProcess), "pages_to_process");
        }

        request.Content = content;

        return await SendRequestAsync<PredictionImageURLActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<AsyncPredictionImageURLActionOutput>> PostImageURLAsync(
        string modelId,
        List<string> urls,
        string? base64Data = null,
        string? requestMetadata = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/v2/OCR/Model/{modelId}/LabelUrls?async=true");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        foreach (var url in urls)
        {
            content.Add(new StringContent(url), "urls");
        }

        if (!string.IsNullOrEmpty(base64Data))
        {
            content.Add(new StringContent(base64Data), "base64_data");
        }

        if (!string.IsNullOrEmpty(requestMetadata))
        {
            content.Add(new StringContent(requestMetadata), "request_metadata");
        }

        request.Content = content;

        return await SendRequestAsync<AsyncPredictionImageURLActionOutput>(
            request,
            cancellationToken);
    }

    public async Task<ApiResponse<UploadTrainingImagesFileActionOutput>> PostTrainingImageFile(
        string modelId,
        string file,
        string? base64Data = null,
        Dictionary<string, string>? requestMetadata = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v2/OCR/Model/{modelId}/TrainingImages");
        
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(file), "file");
        
        if (!string.IsNullOrEmpty(base64Data))
        {
            content.Add(new StringContent(base64Data), "base64_data");
        }
        
        if (requestMetadata != null)
        {
            content.Add(new StringContent(JsonSerializer.Serialize(requestMetadata)), "request_metadata");
        }
        
        request.Content = content;
        
        return await SendRequestAsync<UploadTrainingImagesFileActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<List<TrainingImagesFileDataObject>>> GetTrainingImageFiles(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/OCR/Model/{modelId}/TrainingImages");
        return await SendRequestAsync<List<TrainingImagesFileDataObject>>(request, cancellationToken);
    }

    public async Task<ApiResponse<TrainingImagesFileDataObject>> GetTrainingImageFile(
        string modelId,
        string fileId,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/OCR/Model/{modelId}/TrainingImages/{fileId}");
        return await SendRequestAsync<TrainingImagesFileDataObject>(request, cancellationToken);
    }

    public async Task<ApiResponse<UploadTrainingImagesURLActionOutput>> PostTrainingImageUrls(
        string modelId,
        List<string> urls,
        Dictionary<string, string>? requestMetadata = null,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v2/OCR/Model/{modelId}/UploadUrls/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(JsonSerializer.Serialize(urls)), "urls");
        if (requestMetadata != null)
        {
            content.Add(new StringContent(JsonSerializer.Serialize(requestMetadata)), "request_metadata");
        }
        request.Content = content;

        return await SendRequestAsync<UploadTrainingImagesURLActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<List<TrainingImagesURLDataObject>>> GetTrainingImageUrls(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/OCR/Model/{modelId}/TrainingImages/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await SendRequestAsync<List<TrainingImagesURLDataObject>>(request, cancellationToken);
    }

    public async Task<ApiResponse<TrainTrainModelActionOutput>> TrainModel(
        string modelId,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/OCR/Model/{modelId}/Train");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await SendRequestAsync<TrainTrainModelActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<PredictImageFileActionOutput>> PredictImageFile(
        Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/v2/ImageCategorization/LabelFile/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", "image.jpg");
        request.Content = content;

        return await SendRequestAsync<PredictImageFileActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<PredictImageURLsActionOutput>> PredictImageURLs(
        string modelId,
        List<string> urls,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/ImageCategorization/Model/{modelId}/LabelUrls/");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new MultipartFormDataContent();
        foreach (var url in urls)
        {
            content.Add(new StringContent(url), "urls");
        }
        request.Content = content;

        return await SendRequestAsync<PredictImageURLsActionOutput>(request, cancellationToken);
    }

    public async Task<ApiResponse<IEnumerable<GetAllDataObject>>> GetExternalIntegrations(
        CancellationToken cancellationToken)
    {
        var url = $"{_baseUrl}/api/v2/externalIntegrations";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        
        return await SendRequestAsync<IEnumerable<GetAllDataObject>>(
            request, 
            cancellationToken);
    }

    public async Task<ApiResponse<ExecuteGenericQueryActionOutput>> ExecuteGenericQuery(
        string externalIntegrationId,
        string query,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/v2/externalIntegrations/{externalIntegrationId}/executequery");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var content = new StringContent(
            JsonSerializer.Serialize(new { query }),
            System.Text.Encoding.UTF8,
            "application/json");
        request.Content = content;

        return await SendRequestAsync<ExecuteGenericQueryActionOutput>(
            request, 
            cancellationToken);
    }

    private async Task<ApiResponse<T>> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken);
        return await HandleResponseAsync<T>(response);
    }

    private async Task<ApiResponse<T>> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        return new ApiResponse<T>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Data = response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<T>() : default,
            RawResult = await response.Content.ReadAsStreamAsync()
        };
    }
}