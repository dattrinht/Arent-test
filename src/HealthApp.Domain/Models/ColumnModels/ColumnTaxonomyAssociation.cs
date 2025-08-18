namespace HealthApp.Domain.Models.ColumnModels;

public class ColumnTaxonomyAssociation
{
    [Required]
    public long ProfileId { get; set; }

    public long Id { get; set; }

    [Required]
    public long ColumnId { get; set; }

    [Required]
    public long TaxonomyId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public Column Column { get; set; } = null!;

    public ColumnTaxonomy Taxonomy { get; set; } = null!;
}
