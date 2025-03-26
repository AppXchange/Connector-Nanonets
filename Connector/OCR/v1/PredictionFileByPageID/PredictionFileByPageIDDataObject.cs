namespace Connector.OCR.v1.PredictionFileByPageID;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;
using System.Collections.Generic;

/// <summary>
/// Data object that will represent an object in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
//[AlternateKey("alt-key-id", nameof(CompanyId), nameof(EquipmentNumber))]
[Description("Represents a prediction result for a specific page from the Nanonets OCR API")]
public class PredictionFileByPageIDDataObject
{
    [JsonPropertyName("id")]
    [Description("Unique identifier for this specific page's prediction result")]
    [Required]
    public required string Id { get; init; }

    [JsonPropertyName("message")]
    [Description("Success status of page level prediction")]
    public string? Message { get; init; }

    [JsonPropertyName("input")]
    [Description("Name of the file for which predictions were fetched")]
    public string? Input { get; init; }

    [JsonPropertyName("prediction")]
    [Description("Array of initial predictions from the model")]
    public List<PredictionBox>? Prediction { get; init; }

    [JsonPropertyName("predicted_boxes")]
    [Description("Array of predicted data boxes")]
    public List<PredictionBox>? PredictedBoxes { get; init; }

    [JsonPropertyName("moderated_boxes")]
    [Description("Array of modified data boxes after review")]
    public List<PredictionBox>? ModeratedBoxes { get; init; }

    [JsonPropertyName("raw_ocr")]
    [Description("Array of raw OCR results")]
    public List<OcrResult>? RawOcr { get; init; }

    [JsonPropertyName("page")]
    [Description("Page number in the document (0-based)")]
    public int Page { get; init; }

    [JsonPropertyName("day_since_epoch")]
    [Description("Days since January 1, 1970 (GMT) when file was uploaded")]
    public int DaySinceEpoch { get; init; }

    [JsonPropertyName("request_file_id")]
    [Description("Unique identifier of the uploaded file")]
    public string? RequestFileId { get; init; }

    [JsonPropertyName("is_moderated")]
    [Description("Whether the file is approved (true) or not (false)")]
    public bool IsModerated { get; init; }

    [JsonPropertyName("updated_at")]
    [Description("Timestamp when page details were last updated")]
    public string? UpdatedAt { get; init; }

    [JsonPropertyName("model_id")]
    [Description("Model ID used for predictions")]
    public string? ModelId { get; init; }

    [JsonPropertyName("size")]
    [Description("Dimensions of the page")]
    public ImageSize? Size { get; init; }

    [JsonPropertyName("original_file_name")]
    [Description("Original name of the file")]
    public string? OriginalFileName { get; init; }

    [JsonPropertyName("no_of_fields")]
    [Description("Number of fields detected and processed")]
    public int NumberOfFields { get; init; }

    [JsonPropertyName("export_status")]
    [Description("Status of data export")]
    public string? ExportStatus { get; init; }

    [JsonPropertyName("current_stage_id")]
    [Description("Unique identifier of current processing stage")]
    public string? CurrentStageId { get; init; }

    [JsonPropertyName("approval_status")]
    [Description("Approval status of the file")]
    public string? ApprovalStatus { get; init; }

    [JsonPropertyName("assigned_members")]
    [Description("List of user emails assigned for review")]
    public List<string>? AssignedMembers { get; init; }
}

public class PredictionBox
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("xmin")]
    public int XMin { get; init; }

    [JsonPropertyName("ymin")]
    public int YMin { get; init; }

    [JsonPropertyName("xmax")]
    public int XMax { get; init; }

    [JsonPropertyName("ymax")]
    public int YMax { get; init; }

    [JsonPropertyName("score")]
    public double Score { get; init; }

    [JsonPropertyName("ocr_text")]
    public string? OcrText { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("validation_status")]
    public string? ValidationStatus { get; init; }

    [JsonPropertyName("validation_message")]
    public string? ValidationMessage { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("page")]
    public int Page { get; init; }

    [JsonPropertyName("label_id")]
    public string? LabelId { get; init; }

    [JsonPropertyName("cells")]
    public List<TableCell>? Cells { get; init; }
}

public class TableCell
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("row")]
    public int Row { get; init; }

    [JsonPropertyName("col")]
    public int Col { get; init; }

    [JsonPropertyName("row_span")]
    public int RowSpan { get; init; }

    [JsonPropertyName("col_span")]
    public int ColSpan { get; init; }

    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("xmin")]
    public int XMin { get; init; }

    [JsonPropertyName("ymin")]
    public int YMin { get; init; }

    [JsonPropertyName("xmax")]
    public int XMax { get; init; }

    [JsonPropertyName("ymax")]
    public int YMax { get; init; }

    [JsonPropertyName("score")]
    public double Score { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("verification_status")]
    public string? VerificationStatus { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("failed_validation")]
    public string? FailedValidation { get; init; }
}

public class OcrResult
{
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("xmin")]
    public int XMin { get; init; }

    [JsonPropertyName("ymin")]
    public int YMin { get; init; }

    [JsonPropertyName("xmax")]
    public int XMax { get; init; }

    [JsonPropertyName("ymax")]
    public int YMax { get; init; }
}

public class ImageSize
{
    [JsonPropertyName("width")]
    public int Width { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }
}