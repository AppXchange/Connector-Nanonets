namespace Connector.ImageClassification.v1;
using Connector.ImageClassification.v1.ImageFile;
using Connector.ImageClassification.v1.ImageFile.Predict;
using Connector.ImageClassification.v1.ImageURLs;
using Connector.ImageClassification.v1.ImageURLs.Predict;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class ImageClassificationV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<ImageClassificationV1ActionProcessorConfig>
{
    public override string ModuleId => "imageclassification-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<ImageClassificationV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<ImageClassificationV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<ImageClassificationV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<ImageClassificationV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<ImageClassificationV1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<PredictImageFileHandler>();
        serviceCollection.AddScoped<PredictImageURLsHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, ImageClassificationV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<PredictImageFileHandler, ImageFileDataObject>(ModuleId, "image-file", "predict", config.PredictImageFileConfig);
        service.RegisterHandlerForDataObjectAction<PredictImageURLsHandler, ImageURLsDataObject>(ModuleId, "image-ur-ls", "predict", config.PredictImageURLsConfig);
    }
}