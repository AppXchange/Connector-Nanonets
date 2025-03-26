namespace Connector.ExternalIntegrations.v1.GenericQuery.Execute;

using Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.Action;

/// <summary>
/// Action object that will represent an action in the Xchange system. This will contain an input object type,
/// an output object type, and a Action failure type (this will default to <see cref="StandardActionFailure"/>
/// but that can be overridden with your own preferred type). These objects will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[Description("Action for executing a generic query against an external integration")]
public class ExecuteGenericQueryAction : IStandardAction<ExecuteGenericQueryActionInput, ExecuteGenericQueryActionOutput>
{
    public ExecuteGenericQueryActionInput ActionInput { get; set; } = new() { ExternalIntegrationId = string.Empty, Query = string.Empty };
    public ExecuteGenericQueryActionOutput ActionOutput { get; set; } = new();
    public StandardActionFailure ActionFailure { get; set; } = new();

    public bool CreateRtap => true;
}

public class ExecuteGenericQueryActionInput
{
    [JsonPropertyName("external_integration_id")]
    [Description("ID of the external integration to execute the query against")]
    [Required]
    public required string ExternalIntegrationId { get; set; }

    [JsonPropertyName("query")]
    [Description("The SQL query to execute")]
    [Required]
    public required string Query { get; set; }
}

public class ExecuteGenericQueryActionOutput
{
    [JsonPropertyName("results")]
    [Description("Array of query results")]
    public List<QueryResult>? Results { get; set; }
}

public class QueryResult
{
    [JsonPropertyName("birthplace")]
    [Description("Place of birth")]
    public string? Birthplace { get; set; }

    [JsonPropertyName("dob")]
    [Description("Date of birth")]
    public string? Dob { get; set; }

    [JsonPropertyName("doi")]
    [Description("Date of issue")]
    public string? Doi { get; set; }

    [JsonPropertyName("file")]
    [Description("File identifier")]
    public string? File { get; set; }

    [JsonPropertyName("firstname")]
    [Description("First name")]
    public string? Firstname { get; set; }

    [JsonPropertyName("mrz")]
    [Description("Machine Readable Zone data")]
    public string? Mrz { get; set; }

    [JsonPropertyName("nationality")]
    [Description("Nationality")]
    public string? Nationality { get; set; }

    [JsonPropertyName("passport")]
    [Description("Passport number")]
    public string? Passport { get; set; }

    [JsonPropertyName("sex")]
    [Description("Gender")]
    public string? Sex { get; set; }

    [JsonPropertyName("surname")]
    [Description("Last name")]
    public string? Surname { get; set; }
}
