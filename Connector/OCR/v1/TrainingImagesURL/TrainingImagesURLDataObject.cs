using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Xchange.Connector.SDK.CacheWriter;

namespace Connector.OCR.v1.TrainingImagesURL;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a training image URL in the Nanonets OCR API")]
public class TrainingImagesURLDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the training image URL")]
    [Required]
    public required string Id { get; set; }

    [JsonPropertyName("urls")]
    [Description("Array of publicly hosted file URLs")]
    public List<string> Urls { get; set; } = new();

    [JsonPropertyName("request_metadata")]
    [Description("Metadata to save with the document")]
    public Dictionary<string, string>? RequestMetadata { get; set; }

    [JsonPropertyName("created_at")]
    [Description("Timestamp when the training image URL was created")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    [Description("Timestamp when the training image URL was last updated")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("status")]
    [Description("Current status of the training image URL")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    [Description("Status message or error description")]
    public string? Message { get; set; }

    [JsonPropertyName("signed_urls")]
    [Description("Object containing signed URLs for accessing uploaded files")]
    public Dictionary<string, SignedUrlInfo>? SignedUrls { get; set; }
}

public class SignedUrlInfo
{
    [JsonPropertyName("original")]
    public string? Original { get; set; }

    [JsonPropertyName("original_with_long_expiry")]
    public string? OriginalWithLongExpiry { get; set; }
}