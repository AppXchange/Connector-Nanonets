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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class OCRV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<OCRV1CacheWriterConfig>
{
    public override string ModuleId => "ocr-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<OCRV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<OCRV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<OCRV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<OCRV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<OCRV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<PredictionFilesDataReader>();
        serviceCollection.AddSingleton<PredictionFileByPageIDDataReader>();
        serviceCollection.AddSingleton<PredictionFileByFileIDDataReader>();
        serviceCollection.AddSingleton<ImageFileDataReader>();
        serviceCollection.AddSingleton<ImageURLDataReader>();
        serviceCollection.AddSingleton<TrainingImagesFileDataReader>();
        serviceCollection.AddSingleton<TrainingImagesURLDataReader>();
        serviceCollection.AddSingleton<TrainModelDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<PredictionFilesDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<PredictionFileByPageIDDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<PredictionFileByFileIDDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ImageFileDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ImageURLDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TrainingImagesFileDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TrainingImagesURLDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<TrainModelDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, OCRV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<PredictionFilesDataReader, PredictionFilesDataObject>(ModuleId, config.PredictionFilesConfig, dataReaderSettings);
        service.RegisterDataReader<PredictionFileByPageIDDataReader, PredictionFileByPageIDDataObject>(ModuleId, config.PredictionFileByPageIDConfig, dataReaderSettings);
        service.RegisterDataReader<PredictionFileByFileIDDataReader, PredictionFileByFileIDDataObject>(ModuleId, config.PredictionFileByFileIDConfig, dataReaderSettings);
        service.RegisterDataReader<ImageFileDataReader, ImageFileDataObject>(ModuleId, config.ImageFileConfig, dataReaderSettings);
        service.RegisterDataReader<ImageURLDataReader, ImageURLDataObject>(ModuleId, config.ImageURLConfig, dataReaderSettings);
        service.RegisterDataReader<TrainingImagesFileDataReader, TrainingImagesFileDataObject>(ModuleId, config.TrainingImagesFileConfig, dataReaderSettings);
        service.RegisterDataReader<TrainingImagesURLDataReader, TrainingImagesURLDataObject>(ModuleId, config.TrainingImagesURLConfig, dataReaderSettings);
        service.RegisterDataReader<TrainModelDataReader, TrainModelDataObject>(ModuleId, config.TrainModelConfig, dataReaderSettings);
    }
}