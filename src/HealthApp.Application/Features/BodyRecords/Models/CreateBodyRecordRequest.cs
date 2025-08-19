namespace HealthApp.Application.Features.BodyRecords.Models;

public sealed record CreateBodyRecordRequest(
    [Required, Range(1, long.MaxValue)] long ProfileId,
    [StringLength(256)] string? Title,
    [Required, Range(1, 500)] float Weight,
    [Required, Range(1, 100)] float BodyFat,
    [Required] DateOnly RecordedAt
);