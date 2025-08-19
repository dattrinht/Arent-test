namespace HealthApp.Application.Features.BodyRecords.Models;

public sealed record UpdateBodyRecordRequest(
    [Required, StringLength(256, MinimumLength = 2)] string Title,
    [Range(0.0, 1000.0)] float Weight,
    [Range(0.0, 100.0)] float BodyFat,
    [Required] DateOnly RecordedAt
);