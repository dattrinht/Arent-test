namespace HealthApp.Domain.Models.MealModels.DTOs;

public sealed record MealSummaryDto(
    long Id,
    long ProfileId,
    string? Name,
    EnumMealType Type,
    string? Image,
    DateTime DoneAt
);