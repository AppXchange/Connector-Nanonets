namespace Connector.ExternalIntegrations.v1;
using Connector.ExternalIntegrations.v1.GenericQuery;
using Connector.ExternalIntegrations.v1.GenericQuery.Execute;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Abstraction.Hosting;
using Xchange.Connector.SDK.Action;

public class ExternalIntegrationsV1ActionProcessorServiceDefinition : BaseActionHandlerServiceDefinition<ExternalIntegrationsV1ActionProcessorConfig>
{
    public override string ModuleId => "externalintegrations-1";
    public override Type ServiceType => typeof(GenericActionHandlerService<ExternalIntegrationsV1ActionProcessorConfig>);

    public override void ConfigureServiceDependencies(IServiceCollection serviceCollection, string serviceConfigJson)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        var serviceConfig = JsonSerializer.Deserialize<ExternalIntegrationsV1ActionProcessorConfig>(serviceConfigJson, options);
        serviceCollection.AddSingleton<ExternalIntegrationsV1ActionProcessorConfig>(serviceConfig!);
        serviceCollection.AddSingleton<GenericActionHandlerService<ExternalIntegrationsV1ActionProcessorConfig>>();
        serviceCollection.AddSingleton<IActionHandlerServiceDefinition<ExternalIntegrationsV1ActionProcessorConfig>>(this);
        // Register Action Handlers as scoped dependencies
        serviceCollection.AddScoped<ExecuteGenericQueryHandler>();
    }

    public override void ConfigureService(IActionHandlerService service, ExternalIntegrationsV1ActionProcessorConfig config)
    {
        // Register Action Handler configurations for the Action Processor Service
        service.RegisterHandlerForDataObjectAction<ExecuteGenericQueryHandler, GenericQueryDataObject>(ModuleId, "generic-query", "execute", config.ExecuteGenericQueryConfig);
    }
}