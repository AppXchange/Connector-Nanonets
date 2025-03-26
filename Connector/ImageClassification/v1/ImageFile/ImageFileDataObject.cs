namespace Connector.ImageClassification.v1.ImageFile;

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
[Description("Data object representing an image classification prediction result")]
public class ImageFileDataObject
{
    [JsonPropertyName("model_id")]
    [Description("Unique identifier for the model")]
    [Required]
    public required string ModelId { get; init; }

    [JsonPropertyName("message")]
    [Description("Overall success status of the API call")]
    public string? Message { get; init; }

    [JsonPropertyName("result")]
    [Description("List of prediction results")]
    public List<PredictionResult>? Result { get; init; }
}

public class PredictionResult
{
    [JsonPropertyName("prediction")]
    [Description("List of predictions with labels and probabilities")]
    public List<Prediction>? Prediction { get; init; }

    [JsonPropertyName("file")]
    [Description("Name of the processed file")]
    public string? File { get; init; }

    [JsonPropertyName("message")]
    [Description("Status message for the prediction")]
    public string? Message { get; init; }
}

public class Prediction
{
    [JsonPropertyName("label")]
    [Description("Predicted category label")]
    public string? Label { get; init; }

    [JsonPropertyName("probability")]
    [Description("Confidence score for the prediction")]
    public double Probability { get; init; }
}