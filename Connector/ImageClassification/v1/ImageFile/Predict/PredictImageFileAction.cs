namespace Connector.ImageClassification.v1.ImageFile.Predict;

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
[Description("Action for submitting image files for prediction in the Nanonets Image Classification API")]
public class PredictImageFileAction : IStandardAction<PredictImageFileActionInput, PredictImageFileActionOutput>
{
    public PredictImageFileActionInput ActionInput { get; set; } = new() { File = string.Empty };
    public PredictImageFileActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class PredictImageFileActionInput
{
    [JsonPropertyName("file")]
    [Description("Path to the file stored locally on your system")]
    [Required]
    public required string File { get; set; }
}

public class PredictImageFileActionOutput
{
    [JsonPropertyName("model_id")]
    [Description("Unique identifier for the model")]
    public string? ModelId { get; set; }

    [JsonPropertyName("message")]
    [Description("Overall success status of the API call")]
    public string? Message { get; set; }

    [JsonPropertyName("result")]
    [Description("List of prediction results")]
    public List<PredictionResult>? Result { get; set; }
}

public class PredictionResult
{
    [JsonPropertyName("prediction")]
    [Description("List of predictions with labels and probabilities")]
    public List<Prediction>? Prediction { get; set; }

    [JsonPropertyName("file")]
    [Description("Name of the processed file")]
    public string? File { get; set; }

    [JsonPropertyName("message")]
    [Description("Status message for the prediction")]
    public string? Message { get; set; }
}

public class Prediction
{
    [JsonPropertyName("label")]
    [Description("Predicted category label")]
    public string? Label { get; set; }

    [JsonPropertyName("probability")]
    [Description("Confidence score for the prediction")]
    public double Probability { get; set; }
}
