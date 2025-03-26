namespace Connector.ExternalIntegrations.v1.GenericQuery;

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
[PrimaryKey("file", nameof(File))]
[Description("Data object representing a generic query result")]
public class GenericQueryDataObject
{
    [JsonPropertyName("birthplace")]
    [Description("Place of birth")]
    public string? Birthplace { get; init; }

    [JsonPropertyName("dob")]
    [Description("Date of birth")]
    public string? Dob { get; init; }

    [JsonPropertyName("doi")]
    [Description("Date of issue")]
    public string? Doi { get; init; }

    [JsonPropertyName("file")]
    [Description("File identifier")]
    [Required]
    public required string File { get; init; }

    [JsonPropertyName("firstname")]
    [Description("First name")]
    public string? Firstname { get; init; }

    [JsonPropertyName("mrz")]
    [Description("Machine Readable Zone data")]
    public string? Mrz { get; init; }

    [JsonPropertyName("nationality")]
    [Description("Nationality")]
    public string? Nationality { get; init; }

    [JsonPropertyName("passport")]
    [Description("Passport number")]
    public string? Passport { get; init; }

    [JsonPropertyName("sex")]
    [Description("Gender")]
    public string? Sex { get; init; }

    [JsonPropertyName("surname")]
    [Description("Last name")]
    public string? Surname { get; init; }
}