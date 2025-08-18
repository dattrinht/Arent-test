namespace HealthApp.Application.Features.Columns.Models;

public sealed record UpdateColumnTaxonomyRequest(
    [property: Required, StringLength(256)] string Name
);