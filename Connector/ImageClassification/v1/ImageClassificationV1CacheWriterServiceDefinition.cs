namespace Connector.ImageClassification.v1;
using Connector.ImageClassification.v1.ImageFile;
using Connector.ImageClassification.v1.ImageURLs;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class ImageClassificationV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<ImageClassificationV1CacheWriterConfig>
{
    public override string ModuleId => "imageclassification-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<ImageClassificationV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<ImageClassificationV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<ImageClassificationV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<ImageClassificationV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<ImageClassificationV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<ImageFileDataReader>();
        serviceCollection.AddSingleton<ImageURLsDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<ImageFileDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<ImageURLsDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, ImageClassificationV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<ImageFileDataReader, ImageFileDataObject>(ModuleId, config.ImageFileConfig, dataReaderSettings);
        service.RegisterDataReader<ImageURLsDataReader, ImageURLsDataObject>(ModuleId, config.ImageURLsConfig, dataReaderSettings);
    }
}