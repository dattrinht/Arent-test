namespace HealthApp.Application.Features.Exercises.Models;

public sealed record CreateExerciseRequest(
    [Required, Range(1, long.MaxValue)] long ProfileId,
    [StringLength(128)] string? Name,
    [Required] EnumExerciseType Type,
    [Required] EnumExerciseStatus Status,
    [Range(0, int.MaxValue)] int DurationSec,
    [Range(0, int.MaxValue)] int CaloriesBurned,
    [Required] DateTime FinishedAt
);
