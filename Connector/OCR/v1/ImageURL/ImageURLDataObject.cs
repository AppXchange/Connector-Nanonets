namespace Connector.OCR.v1.ImageURL;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Xchange.Connector.SDK.CacheWriter;
using Connector.OCR.v1.ImageURL.AsyncPrediction;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents an image URL for prediction in the Nanonets OCR API")]
public class ImageURLDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the prediction")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("message")]
    [Description("Success status of the prediction")]
    public string? Message { get; init; }

    [JsonPropertyName("input")]
    [Description("Name of the file uploaded to the model")]
    public string? Input { get; init; }

    [JsonPropertyName("prediction")]
    [Description("Array of predictions")]
    public List<object>? Prediction { get; init; }

    [JsonPropertyName("page")]
    [Description("Page number in the document (0-based)")]
    public int Page { get; init; }

    [JsonPropertyName("request_file_id")]
    [Description("Unique identifier of the uploaded file")]
    public string? RequestFileId { get; init; }

    [JsonPropertyName("filepath")]
    [Description("Path to the uploaded file")]
    public string? Filepath { get; init; }

    [JsonPropertyName("rotation")]
    [Description("Rotation angle of the image")]
    public int Rotation { get; init; }

    [JsonPropertyName("file_url")]
    [Description("URL to the original file")]
    public string? FileUrl { get; init; }

    [JsonPropertyName("request_metadata")]
    [Description("Metadata associated with the request")]
    public string? RequestMetadata { get; init; }

    [JsonPropertyName("processing_type")]
    [Description("Type of processing (async or sync)")]
    public string? ProcessingType { get; init; }

    [JsonPropertyName("size")]
    [Description("Dimensions of the image")]
    public ImageSize? Size { get; init; }

    [JsonPropertyName("raw_ocr_api_response")]
    [Description("Raw OCR API response")]
    public RawOcrApiResponse? RawOcrApiResponse { get; init; }
}