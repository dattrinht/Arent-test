namespace HealthApp.Application.Features.Exercises.Models;

public sealed record UpdateExerciseRequest(
    [property: StringLength(128)] string? Name,
    [property: Required] EnumExerciseType Type,
    [property: Required] EnumExerciseStatus Status,
    [property: Range(0, int.MaxValue)] int DurationSec,
    [property: Range(0, int.MaxValue)] int CaloriesBurned,
    [property: Required] DateTime FinishedAt
);
