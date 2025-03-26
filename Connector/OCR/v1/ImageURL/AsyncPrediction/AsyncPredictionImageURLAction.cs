namespace Connector.OCR.v1.ImageURL.AsyncPrediction;

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
[Description("Action for submitting image URLs for asynchronous prediction in the Nanonets OCR API")]
public class AsyncPredictionImageURLAction : IStandardAction<AsyncPredictionImageURLActionInput, AsyncPredictionImageURLActionOutput>
{
    public AsyncPredictionImageURLActionInput ActionInput { get; set; } = new() { Urls = new List<string>() };
    public AsyncPredictionImageURLActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class AsyncPredictionImageURLActionInput
{
    [JsonPropertyName("urls")]
    [Description("Array of publicly hosted file URLs")]
    [Required]
    public required List<string> Urls { get; set; }

    [JsonPropertyName("base64_data")]
    [Description("Base64 encoded version of the file URLs")]
    public string? Base64Data { get; set; }

    [JsonPropertyName("request_metadata")]
    [Description("Metadata to save with the document")]
    public string? RequestMetadata { get; set; }
}

public class AsyncPredictionImageURLActionOutput
{
    [JsonPropertyName("message")]
    [Description("Overall success status of the API call")]
    public string? Message { get; set; }

    [JsonPropertyName("result")]
    [Description("List of objects representing each page of the processed files")]
    public List<PredictionResult>? Result { get; set; }

    [JsonPropertyName("signed_urls")]
    [Description("Object containing signed URLs for accessing processed files")]
    public Dictionary<string, SignedUrlInfo>? SignedUrls { get; set; }
}

public class PredictionResult
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("input")]
    public string? Input { get; set; }

    [JsonPropertyName("prediction")]
    public List<PredictionBox>? Prediction { get; set; }

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

    [JsonPropertyName("raw_ocr_api_response")]
    public RawOcrApiResponse? RawOcrApiResponse { get; set; }
}

public class ImageSize
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class RawOcrApiResponse
{
    [JsonPropertyName("results")]
    public object? Results { get; set; }
}

public class SignedUrlInfo
{
    [JsonPropertyName("original")]
    public string? Original { get; set; }

    [JsonPropertyName("original_with_long_expiry")]
    public string? OriginalWithLongExpiry { get; set; }
}

public class PredictionBox
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("xmin")]
    public int XMin { get; set; }

    [JsonPropertyName("ymin")]
    public int YMin { get; set; }

    [JsonPropertyName("xmax")]
    public int XMax { get; set; }

    [JsonPropertyName("ymax")]
    public int YMax { get; set; }

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("ocr_text")]
    public string? OcrText { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("validation_status")]
    public string? ValidationStatus { get; set; }

    [JsonPropertyName("validation_message")]
    public string? ValidationMessage { get; set; }

    [JsonPropertyName("page_no")]
    public int PageNo { get; set; }

    [JsonPropertyName("label_id")]
    public string? LabelId { get; set; }

    [JsonPropertyName("cells")]
    public List<TableCell>? Cells { get; set; }
}

public class TableCell
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("row")]
    public int Row { get; set; }

    [JsonPropertyName("col")]
    public int Col { get; set; }

    [JsonPropertyName("row_span")]
    public int RowSpan { get; set; }

    [JsonPropertyName("col_span")]
    public int ColSpan { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("xmin")]
    public int XMin { get; set; }

    [JsonPropertyName("ymin")]
    public int YMin { get; set; }

    [JsonPropertyName("xmax")]
    public int XMax { get; set; }

    [JsonPropertyName("ymax")]
    public int YMax { get; set; }

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("verification_status")]
    public string? VerificationStatus { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("failed_validation")]
    public string? FailedValidation { get; set; }

    [JsonPropertyName("label_id")]
    public string? LabelId { get; set; }
}
