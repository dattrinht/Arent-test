namespace HealthApp.Application.Features.Columns.Models;

public sealed record CreateColumnTaxonomyRequest(
    [property: Required, Range(1, long.MaxValue)] long ProfileId,
    [property: Required, StringLength(256)] string Name,
    [property: Required] EnumTaxonomyType Type
);
