namespace Connector.OCR.v1;
using Connector.OCR.v1.ImageFile;
using Connector.OCR.v1.ImageURL;
using Connector.OCR.v1.PredictionFileByFileID;
using Connector.OCR.v1.PredictionFileByPageID;
using Connector.OCR.v1.PredictionFiles;
using Connector.OCR.v1.TrainingImagesFile;
using Connector.OCR.v1.TrainingImagesURL;
using Connector.OCR.v1.TrainModel;
using ESR.Hosting.CacheWriter;
using Json.Schema.Generation;

/// <summary>
/// Configuration for the Cache writer for this module. This configuration will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// The schema will be used for validation at runtime to make sure the configurations are properly formed. 
/// The schema also helps provide integrators more information for what the values are intended to be.
/// </summary>
[Title("OCR V1 Cache Writer Configuration")]
[Description("Configuration of the data object caches for the module.")]
public class OCRV1CacheWriterConfig
{
    // Data Reader configuration
    public CacheWriterObjectConfig PredictionFilesConfig { get; set; } = new();
    public CacheWriterObjectConfig PredictionFileByPageIDConfig { get; set; } = new();
    public CacheWriterObjectConfig PredictionFileByFileIDConfig { get; set; } = new();
    public CacheWriterObjectConfig ImageFileConfig { get; set; } = new();
    public CacheWriterObjectConfig ImageURLConfig { get; set; } = new();
    public CacheWriterObjectConfig TrainingImagesFileConfig { get; set; } = new();
    public CacheWriterObjectConfig TrainingImagesURLConfig { get; set; } = new();
    public CacheWriterObjectConfig TrainModelConfig { get; set; } = new();
}