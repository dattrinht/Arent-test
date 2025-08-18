namespace HealthApp.Domain.Models.ColumnModels.DTOs;

public record ColumnSummaryDto(
    long Id,
    string Slug,
    string Title,
    string Summary,
    string? DisplayImage,
    bool IsPublished,
    DateTime CreatedAt,
    DateTime? PublishedAt

)
{
    public IReadOnlyList<ColumnTaxonomySummaryDto>? Taxonomies { get; set; }
}