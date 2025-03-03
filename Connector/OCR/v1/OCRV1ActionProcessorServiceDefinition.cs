namespace Connector.OCR.v1;
using Connector.OCR.v1.ImageFile;
using Connector.OCR.v1.ImageFile.AsyncPrediction;
using Connector.OCR.v1.ImageFile.Prediction;
using Connector.OCR.v1.ImageURL;
using Connector.OCR.v1.ImageURL.AsyncPrediction;
using Connector.OCR.v1.ImageURL.Prediction;
using Connector.OCR.v1.TrainingImagesFile;
using Connector.OCR.v1.TrainingImagesFile.Upload;
using Connector.OCR.v1.TrainingImagesURL;
using Connector.OCR.v1.TrainingImagesURL.Upload;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class OCRV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<OCRV1ActionProcessorConfig>
{
    public override string ModuleId => "ocr-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<OCRV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<OCRV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<OCRV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<OCRV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<OCRV1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<AsyncPredictionImageFileHandler>();
        serviceCollection.AddScoped<AsyncPredictionImageURLHandler>();
        serviceCollection.AddScoped<PredictionImageFileHandler>();
        serviceCollection.AddScoped<PredictionImageURLHandler>();
        serviceCollection.AddScoped<UploadTrainingImagesFileHandler>();
        serviceCollection.AddScoped<UploadTrainingImagesURLHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, OCRV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<AsyncPredictionImageFileHandler, ImageFileDataObject>(ModuleId, "image-file", "async-prediction", config.AsyncPredictionImageFileConfig);
        service.RegisterHandlerForDataObjectAction<AsyncPredictionImageURLHandler, ImageURLDataObject>(ModuleId, "image-url", "async-prediction", config.AsyncPredictionImageURLConfig);
        service.RegisterHandlerForDataObjectAction<PredictionImageFileHandler, ImageFileDataObject>(ModuleId, "image-file", "prediction", config.PredictionImageFileConfig);
        service.RegisterHandlerForDataObjectAction<PredictionImageURLHandler, ImageURLDataObject>(ModuleId, "image-url", "prediction", config.PredictionImageURLConfig);
        service.RegisterHandlerForDataObjectAction<UploadTrainingImagesFileHandler, TrainingImagesFileDataObject>(ModuleId, "training-images-file", "upload", config.UploadTrainingImagesFileConfig);
        service.RegisterHandlerForDataObjectAction<UploadTrainingImagesURLHandler, TrainingImagesURLDataObject>(ModuleId, "training-images-url", "upload", config.UploadTrainingImagesURLConfig);
    }
}