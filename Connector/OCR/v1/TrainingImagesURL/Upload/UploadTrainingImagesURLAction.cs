namespace Connector.OCR.v1.TrainingImagesURL.Upload;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that will represent an action in the Xchange system. This will contain an input object type,
/// an output object type, and a Action failure type (this will default to <see cref="StandardActionFailure"/>
/// but that can be overridden with your own preferred type). These objects will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[Description("Action for uploading training image URLs to the Nanonets OCR API")]
public class UploadTrainingImagesURLAction : IStandardAction<UploadTrainingImagesURLActionInput, UploadTrainingImagesURLActionOutput>
{
    public UploadTrainingImagesURLActionInput ActionInput { get; set; } = new() { Urls = new List<string>() };
    public UploadTrainingImagesURLActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class UploadTrainingImagesURLActionInput
{
    [JsonPropertyName("urls")]
    [Description("Array of publicly hosted file URLs")]
    [Required]
    public required List<string> Urls { get; set; }

    [JsonPropertyName("request_metadata")]
    [Description("Metadata to save with the document")]
    public Dictionary<string, string>? RequestMetadata { get; set; }
}

public class UploadTrainingImagesURLActionOutput
{
    [JsonPropertyName("message")]
    [Description("Overall success status of the API call")]
    public string? Message { get; set; }

    [JsonPropertyName("result")]
    [Description("List of objects representing each uploaded file")]
    public List<UploadResult>? Result { get; set; }

    [JsonPropertyName("signed_urls")]
    [Description("Object containing signed URLs for accessing uploaded files")]
    public Dictionary<string, SignedUrlInfo>? SignedUrls { get; set; }
}

public class UploadResult
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("input")]
    public string? Input { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("request_file_id")]
    public string? RequestFileId { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("request_metadata")]
    public string? RequestMetadata { get; set; }

    [JsonPropertyName("processing_type")]
    public string? ProcessingType { get; set; }

    [JsonPropertyName("size")]
    public ImageSize? Size { get; set; }

    [JsonPropertyName("filepath")]
    public string? Filepath { get; set; }

    [JsonPropertyName("rotation")]
    public int Rotation { get; set; }

    [JsonPropertyName("file_url")]
    public string? FileUrl { get; set; }
}

public class ImageSize
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class SignedUrlInfo
{
    [JsonPropertyName("original")]
    public string? Original { get; set; }

    [JsonPropertyName("original_with_long_expiry")]
    public string? OriginalWithLongExpiry { get; set; }
}
