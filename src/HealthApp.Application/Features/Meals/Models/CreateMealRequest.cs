namespace HealthApp.Application.Features.Meals.Models;

public sealed record CreateMealRequest(
    long ProfileId,
    string Name,
    EnumMealType Type,
    string? Image,
    DateTime DoneAt
);
