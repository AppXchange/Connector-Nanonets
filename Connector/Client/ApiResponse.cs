using System.IO;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Connector.OCR.v1.PredictionFiles;
using Connector.OCR.v1.PredictionFileByPageID;
using Connector.OCR.v1.PredictionFileByFileID;

namespace Connector.Client;

public class ApiResponse
{
    public bool IsSuccessful { get; init; }
    public int StatusCode { get; init; }
    // Not safe to read if `Data` is not null
    public Stream? RawResult { get; init; }
}

public class ApiResponse<TResult> : ApiResponse
{
    public TResult? Data { get; init; }
}

public class PredictionFilesResponse
{
    [JsonPropertyName("moderated_images_count")]
    public int ModeratedImagesCount { get; init; }

    [JsonPropertyName("unmoderated_images_count")]
    public int UnmoderatedImagesCount { get; init; }

    [JsonPropertyName("moderated_images")]
    public List<PredictionFilesDataObject>? ModeratedImages { get; init; }

    [JsonPropertyName("unmoderated_images")]
    public List<PredictionFilesDataObject>? UnmoderatedImages { get; init; }
}

public class PredictionFileByPageIDResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("result")]
    public List<PredictionFileByPageIDDataObject>? Result { get; init; }

    [JsonPropertyName("signed_urls")]
    public Dictionary<string, string>? SignedUrls { get; init; }
}

public class PredictionFileByFileIDResponse
{
    [JsonPropertyName("moderated_images_count")]
    public int ModeratedImagesCount { get; init; }

    [JsonPropertyName("unmoderated_images_count")]
    public int UnmoderatedImagesCount { get; init; }

    [JsonPropertyName("moderated_images")]
    public List<PredictionFileByFileIDDataObject>? ModeratedImages { get; init; }

    [JsonPropertyName("unmoderated_images")]
    public List<PredictionFileByFileIDDataObject>? UnmoderatedImages { get; init; }

    [JsonPropertyName("signed_urls")]
    public Dictionary<string, string>? SignedUrls { get; init; }
}