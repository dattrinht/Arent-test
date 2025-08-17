namespace HealthApp.Application.Features.Meals.Models;

public sealed record UpdateMealRequest(
    [property: Required, StringLength(200, MinimumLength = 1)] string Name,
    [property: Required] EnumMealType Type,
    [property: StringLength(500)] string? Image,
    [property: Required] DateTime DoneAt
);
