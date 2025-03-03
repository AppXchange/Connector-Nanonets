namespace Connector.OCR.v1;
using Connector.OCR.v1.ImageFile.AsyncPrediction;
using Connector.OCR.v1.ImageFile.Prediction;
using Connector.OCR.v1.ImageURL.AsyncPrediction;
using Connector.OCR.v1.ImageURL.Prediction;
using Connector.OCR.v1.TrainingImagesFile.Upload;
using Connector.OCR.v1.TrainingImagesURL.Upload;
using Json.Schema.Generation;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Configuration for the Action Processor for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("OCR V1 Action Processor Configuration")]
[Description("Configuration of the data object actions for the module.")]
public class OCRV1ActionProcessorConfig
{
    // Action Handler configuration
    public DefaultActionHandlerConfig AsyncPredictionImageFileConfig { get; set; } = new();
    public DefaultActionHandlerConfig AsyncPredictionImageURLConfig { get; set; } = new();
    public DefaultActionHandlerConfig PredictionImageFileConfig { get; set; } = new();
    public DefaultActionHandlerConfig PredictionImageURLConfig { get; set; } = new();
    public DefaultActionHandlerConfig UploadTrainingImagesFileConfig { get; set; } = new();
    public DefaultActionHandlerConfig UploadTrainingImagesURLConfig { get; set; } = new();
}