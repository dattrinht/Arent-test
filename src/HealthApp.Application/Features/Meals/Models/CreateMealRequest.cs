namespace HealthApp.Application.Features.Meals.Models;

public sealed record CreateMealRequest(
    [Required, Range(1, long.MaxValue)] long ProfileId,
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [Required] EnumMealType Type,
    [StringLength(500)] string? Image,
    [Required] DateTime DoneAt
);
