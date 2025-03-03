namespace Connector.ExternalIntegrations.v1;
using Connector.ExternalIntegrations.v1.GenericQuery;
using Connector.ExternalIntegrations.v1.GetAll;
using ESR.Hosting.CacheWriter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using Xchange.Connector.SDK.Abstraction.Change;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.CacheWriter;
using Xchange.Connector.SDK.Hosting.Configuration;

public class ExternalIntegrationsV1CacheWriterServiceDefinition : BaseCacheWriterServiceDefinition<ExternalIntegrationsV1CacheWriterConfig>
{
    public override string ModuleId => "externalintegrations-1";
    public override Type ServiceType => typeof(GenericCacheWriterService<ExternalIntegrationsV1CacheWriterConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var serviceConfig = JsonSerializer.Deserialize<ExternalIntegrationsV1CacheWriterConfig>(serviceConfigJson);
        serviceCollection.AddSingleton<ExternalIntegrationsV1CacheWriterConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericCacheWriterService<ExternalIntegrationsV1CacheWriterConfig>>();
        serviceCollection.AddSingleton<ICacheWriterServiceDefinition<ExternalIntegrationsV1CacheWriterConfig>>(this);
        // Register Data Readers as Singletons
        serviceCollection.AddSingleton<GetAllDataReader>();
        serviceCollection.AddSingleton<GenericQueryDataReader>();
    }

    public override IDataObjectChangeDetectorProvider ConfigureChangeDetectorProvider(IChangeDetectorFactory factory, ConnectorDefinition connectorDefinition)
    {
        var options = factory.CreateProviderOptionsWithNoDefaultResolver();
        // Configure Data Object Keys for Data Objects that do not use the default
        this.RegisterKeysForObject<GetAllDataObject>(options, connectorDefinition);
        this.RegisterKeysForObject<GenericQueryDataObject>(options, connectorDefinition);
        return factory.CreateProvider(options);
    }

    public override void ConfigureService(ICacheWriterService service, ExternalIntegrationsV1CacheWriterConfig config)
    {
        var dataReaderSettings = new DataReaderSettings
        {
            DisableDeletes = false,
            UseChangeDetection = true
        };
        // Register Data Reader configurations for the Cache Writer Service
        service.RegisterDataReader<GetAllDataReader, GetAllDataObject>(ModuleId, config.GetAllConfig, dataReaderSettings);
        service.RegisterDataReader<GenericQueryDataReader, GenericQueryDataObject>(ModuleId, config.GenericQueryConfig, dataReaderSettings);
    }
}