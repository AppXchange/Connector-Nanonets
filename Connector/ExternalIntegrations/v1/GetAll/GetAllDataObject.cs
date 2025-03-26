namespace Connector.ExternalIntegrations.v1.GetAll;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents an external integration in the Nanonets system")]
public class GetAllDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for the external integration")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("email")]
    [Description("Email associated with the integration")]
    public string? Email { get; init; }

    [JsonPropertyName("name")]
    [Description("Name of the integration")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    [Description("Type of integration (e.g., postgresql)")]
    public string? Type { get; init; }

    [JsonPropertyName("info")]
    [Description("Integration-specific information")]
    public IntegrationInfo? Info { get; init; }

    [JsonPropertyName("status")]
    [Description("Current status of the integration")]
    public string? Status { get; init; }

    [JsonPropertyName("is_internal")]
    [Description("Whether this is an internal integration")]
    public bool IsInternal { get; init; }
}

public class IntegrationInfo
{
    [JsonPropertyName("db_name")]
    [Description("Name of the database")]
    public string? DbName { get; init; }

    [JsonPropertyName("host")]
    [Description("Host address")]
    public string? Host { get; init; }

    [JsonPropertyName("password")]
    [Description("Database password")]
    public string? Password { get; init; }

    [JsonPropertyName("username")]
    [Description("Database username")]
    public string? Username { get; init; }
}