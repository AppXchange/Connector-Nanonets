namespace Connector.OCR.v1.TrainModel.Train;

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
[Description("Action for training a model in the Nanonets OCR API")]
public class TrainTrainModelAction : IStandardAction<TrainTrainModelActionInput, TrainTrainModelActionOutput>
{
    public TrainTrainModelActionInput ActionInput { get; set; } = new() { ModelId = string.Empty };
    public TrainTrainModelActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class TrainTrainModelActionInput
{
    [JsonPropertyName("model_id")]
    [Description("Unique identifier for the model to train")]
    [Required]
    public required string ModelId { get; set; }
}

public class TrainTrainModelActionOutput
{
    [JsonPropertyName("model_id")]
    [Description("Unique identifier for the trained model")]
    public string? ModelId { get; set; }

    [JsonPropertyName("model_type")]
    [Description("Type of the model (e.g., 'ocr')")]
    public string? ModelType { get; set; }

    [JsonPropertyName("state")]
    [Description("Current state of the model")]
    public int State { get; set; }

    [JsonPropertyName("categories")]
    [Description("List of categories in the model")]
    public List<Category>? Categories { get; set; }
}

public class Category
{
    [JsonPropertyName("name")]
    [Description("Name of the category")]
    public string? Name { get; set; }

    [JsonPropertyName("count")]
    [Description("Number of images in this category")]
    public int Count { get; set; }
}
