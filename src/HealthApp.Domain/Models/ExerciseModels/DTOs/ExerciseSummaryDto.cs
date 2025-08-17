namespace HealthApp.Domain.Models.ExerciseModels.DTOs;

public sealed record ExerciseSummaryDto(
    long Id,
    long ProfileId,
    string? Name,
    EnumExerciseType Type,
    EnumExerciseStatus Status,
    int DurationSec,
    int CaloriesBurned,
    DateTime FinishedAt
);
