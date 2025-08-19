namespace HealthApp.Application.Features.Columns.Models;

public sealed record CreateColumnRequest(
    [Required, StringLength(256)] string Slug,
    [Required, StringLength(512)] string Title,
    [StringLength(4000)] string? Summary,
    [Required] string Content,
    [StringLength(512)] string? DisplayImage,
    [Required] bool IsPublished,
    IReadOnlyList<long>? TaxonomyIds
);
