namespace HealthApp.Application.Features.BodyRecords.Models;

public sealed record UpdateBodyRecordRequest(
    [property: Required, StringLength(256, MinimumLength = 2)] string Title,
    [property: Range(0.0, 1000.0)] float Weight,
    [property: Range(0.0, 100.0)] float BodyFat,
    [property: Required] DateOnly RecordedAt
);