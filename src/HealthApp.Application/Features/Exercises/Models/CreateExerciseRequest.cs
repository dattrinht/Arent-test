namespace HealthApp.Application.Features.Exercises.Models;

public sealed record CreateExerciseRequest(
    [property: Required, Range(1, long.MaxValue)] long ProfileId,
    [property: StringLength(128)] string? Name,
    [property: Required] EnumExerciseType Type,
    [property: Required] EnumExerciseStatus Status,
    [property: Range(0, int.MaxValue)] int DurationSec,
    [property: Range(0, int.MaxValue)] int CaloriesBurned,
    [property: Required] DateTime FinishedAt
);
