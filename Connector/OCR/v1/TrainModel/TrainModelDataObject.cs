namespace Connector.OCR.v1.TrainModel;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("model_id", nameof(ModelId))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a trained model in the Nanonets OCR API")]
public class TrainModelDataObject
{
    [JsonPropertyName("model_id")]
    [Description("Unique identifier for the model")]
    [Required]
    public required string ModelId { get; init; }

    [JsonPropertyName("model_type")]
    [Description("Type of the model (e.g., 'ocr')")]
    public string? ModelType { get; init; }

    [JsonPropertyName("state")]
    [Description("Current state of the model")]
    public int State { get; init; }

    [JsonPropertyName("categories")]
    [Description("List of categories in the model")]
    public List<Category>? Categories { get; init; }
}

public class Category
{
    [JsonPropertyName("name")]
    [Description("Name of the category")]
    public string? Name { get; init; }

    [JsonPropertyName("count")]
    [Description("Number of images in this category")]
    public int Count { get; init; }
}