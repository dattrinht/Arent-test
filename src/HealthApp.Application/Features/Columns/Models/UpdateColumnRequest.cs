namespace HealthApp.Application.Features.Columns.Models;

public sealed record UpdateColumnRequest(
    [property: Required, StringLength(256)] string Slug,
    [property: Required, StringLength(512)] string Title,
    [property: StringLength(4000)] string? Summary,
    [property: Required] string Content,
    [property: StringLength(512)] string? DisplayImage,
    [property: Required] bool IsPublished,
    IReadOnlyList<long>? TaxonomyIds
);