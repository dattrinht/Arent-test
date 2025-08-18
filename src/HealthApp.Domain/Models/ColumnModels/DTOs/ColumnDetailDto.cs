namespace HealthApp.Domain.Models.ColumnModels.DTOs;

public record ColumnDetailDto(
    long Id,
    string Slug,
    string Title,
    string Summary,
    string Content,
    string? DisplayImage,
    bool IsPublished,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PublishedAt,
    IReadOnlyList<ColumnTaxonomySummaryDto>? Taxonomies
);