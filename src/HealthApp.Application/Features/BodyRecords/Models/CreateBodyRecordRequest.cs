namespace HealthApp.Application.Features.BodyRecords.Models;

public sealed record CreateBodyRecordRequest(
    [property: Required, Range(1, long.MaxValue)] long ProfileId,
    [property: StringLength(256)] string? Title,
    [property: Required, Range(1, 500)] float Weight,
    [property: Required, Range(1, 100)] float BodyFat,
    [property: Required] DateOnly RecordedAt
);