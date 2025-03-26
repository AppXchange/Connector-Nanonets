namespace Connector.OCR.v1.PredictionFiles;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a prediction file from the Nanonets OCR API")]
public class PredictionFilesDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the prediction")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("model_id")]
    [Description("The model ID used for making predictions")]
    public string? ModelId { get; init; }

    [JsonPropertyName("day_since_epoch")]
    [Description("Days since January 1, 1970 (GMT) when the file was uploaded")]
    public int DaySinceEpoch { get; init; }

    [JsonPropertyName("is_moderated")]
    [Description("Whether the file has been approved (true) or not (false)")]
    public bool IsModerated { get; init; }

    [JsonPropertyName("url")]
    [Description("Path to the uploaded image file")]
    public string? Url { get; init; }

    [JsonPropertyName("file_url")]
    [Description("Path to the original uploaded file")]
    public string? FileUrl { get; init; }

    [JsonPropertyName("predicted_boxes")]
    [Description("Array of predicted bounding boxes and their details")]
    public List<PredictedBox>? PredictedBoxes { get; init; }

    [JsonPropertyName("size")]
    [Description("Dimensions of the image/page")]
    public ImageSize? Size { get; init; }

    [JsonPropertyName("page")]
    [Description("Page number in the document")]
    public int Page { get; init; }

    [JsonPropertyName("request_file_id")]
    [Description("Unique identifier of the uploaded file")]
    public string? RequestFileId { get; init; }

    [JsonPropertyName("original_file_name")]
    [Description("Original name of the uploaded file")]
    public string? OriginalFileName { get; init; }

    [JsonPropertyName("approval_status")]
    [Description("Approval status of the file (approved, rejected, or blank)")]
    public string? ApprovalStatus { get; init; }

    [JsonPropertyName("processing_type")]
    [Description("How the file was processed (async or sync)")]
    public string? ProcessingType { get; init; }
}

public class PredictedBox
{
    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("xmin")]
    public int XMin { get; init; }

    [JsonPropertyName("ymin")]
    public int YMin { get; init; }

    [JsonPropertyName("xmax")]
    public int XMax { get; init; }

    [JsonPropertyName("ymax")]
    public int YMax { get; init; }

    [JsonPropertyName("ocr_text")]
    public string? OcrText { get; init; }

    [JsonPropertyName("score")]
    public double Score { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

public class ImageSize
{
    [JsonPropertyName("width")]
    public int Width { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }
}