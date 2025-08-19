namespace HealthApp.Application.Features.Columns.Models;

public sealed record UpdateColumnTaxonomyRequest(
    [Required, StringLength(256)] string Name
);