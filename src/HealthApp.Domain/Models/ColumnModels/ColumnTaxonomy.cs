namespace HealthApp.Domain.Models.ColumnModels;

public class ColumnTaxonomy : Entity
{
    [Required]
    public long ProfileId { get; set; }

    [Required, StringLength(256)]
    public string Name { get; set; } = null!;

    [Required]
    public EnumTaxonomyType Type { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<ColumnTaxonomyAssociation> ColumnTaxonomies { get; set; } = [];
}
