namespace HealthApp.Domain.Models.BodyRecordModels.DTOs;

public sealed record BodyRecordSummaryDto(
    long Id,
    string? Title,
    float Weight,
    float BodyFat,
    DateOnly RecordedAt
);