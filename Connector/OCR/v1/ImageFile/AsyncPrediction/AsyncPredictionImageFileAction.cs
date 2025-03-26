namespace Connector.OCR.v1.ImageFile.AsyncPrediction;

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
[Description("Action for submitting image files for asynchronous prediction in the Nanonets OCR API")]
public class AsyncPredictionImageFileAction : IStandardAction<AsyncPredictionImageFileActionInput, AsyncPredictionImageFileActionOutput>
{
    public AsyncPredictionImageFileActionInput ActionInput { get; set; } = new() { File = string.Empty };
    public AsyncPredictionImageFileActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class AsyncPredictionImageFileActionInput
{
    [JsonPropertyName("file")]
    [Description("Path to the file stored locally on your system")]
    [Required]
    public required string File { get; set; }

    [JsonPropertyName("base64_data")]
    [Description("Base64 encoded version of the file")]
    public string? Base64Data { get; set; }

    [JsonPropertyName("request_metadata")]
    [Description("Metadata to save with the document")]
    public string? RequestMetadata { get; set; }
}

public class AsyncPredictionImageFileActionOutput
{
    [JsonPropertyName("message")]
    [Description("Overall success status of the API call")]
    public string? Message { get; set; }

    [JsonPropertyName("result")]
    [Description("List of objects representing each page of the processed files")]
    public List<ImageFileDataObject>? Result { get; set; }

    [JsonPropertyName("signed_urls")]
    [Description("Object containing signed URLs for accessing processed files")]
    public Dictionary<string, SignedUrlInfo>? SignedUrls { get; set; }
}

public class SignedUrlInfo
{
    [JsonPropertyName("original")]
    [Description("Original URL with 4-hour expiry")]
    public string? Original { get; set; }

    [JsonPropertyName("original_with_long_expiry")]
    [Description("Original URL with 180-day expiry")]
    public string? OriginalWithLongExpiry { get; set; }
}
