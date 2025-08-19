namespace HealthApp.Application.Features.Exercises.Models;

public sealed record UpdateExerciseRequest(
    [StringLength(128)] string? Name,
    [Required] EnumExerciseType Type,
    [Required] EnumExerciseStatus Status,
    [Range(0, int.MaxValue)] int DurationSec,
    [Range(0, int.MaxValue)] int CaloriesBurned,
    [Required] DateTime FinishedAt
);
