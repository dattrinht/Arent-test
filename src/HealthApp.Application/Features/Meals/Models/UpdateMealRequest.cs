namespace HealthApp.Application.Features.Meals.Models;

public sealed record UpdateMealRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Name,
    [Required] EnumMealType Type,
    [StringLength(500)] string? Image,
    [Required] DateTime DoneAt
);
