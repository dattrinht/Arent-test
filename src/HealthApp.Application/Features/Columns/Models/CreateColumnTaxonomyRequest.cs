namespace HealthApp.Application.Features.Columns.Models;

public sealed record CreateColumnTaxonomyRequest(
    [Required, Range(1, long.MaxValue)] long ProfileId,
    [Required, StringLength(256)] string Name,
    [Required] EnumTaxonomyType Type
);
