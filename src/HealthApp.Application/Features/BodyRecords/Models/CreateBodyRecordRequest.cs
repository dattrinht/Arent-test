namespace HealthApp.Application.Features.BodyRecords.Models;

public sealed record CreateBodyRecordRequest(
    long ProfileId,
    string? Title,
    float Weight,
    float BodyFat,
    DateOnly RecordedAt
);