namespace HealthApp.Domain.Models.ColumnModels;

public class Column : Entity
{
    [Required]
    public long ProfileId { get; set; }

    [Required, StringLength(256)]
    public required string Slug { get; set; }

    [Required, StringLength(512)]
    public required string Title { get; set; }

    [StringLength(4000)]
    public required string Summary { get; set; }

    [Required]
    public required string Content { get; set; }

    [StringLength(512)]
    public string? DisplayImage { get; set; }

    public bool IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<ColumnTaxonomyAssociation> ColumnTaxonomies { get; set; } = [];
}
