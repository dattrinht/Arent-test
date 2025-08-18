namespace HealthApp.Infrastructure.EntityConfigurations;

internal sealed class ColumnTaxonomyAssociationEntityTypeConfiguration : IEntityTypeConfiguration<ColumnTaxonomyAssociation>
{
    public void Configure(EntityTypeBuilder<ColumnTaxonomyAssociation> b)
    {
        b.ToTable("column_taxonomy_associations");

        b.HasKey(x => x.Id);

        b.HasOne(x => x.Column)
            .WithMany(c => c.ColumnTaxonomies)
            .HasForeignKey(x => x.ColumnId);

        b.HasOne(x => x.Taxonomy)
            .WithMany(t => t.ColumnTaxonomies)
            .HasForeignKey(x => x.TaxonomyId);

        b.HasIndex(x => new { x.ColumnId, x.TaxonomyId })
            .IsUnique()
            .HasDatabaseName("ux_column_taxonomy_association");

        b.HasIndex(x => x.TaxonomyId)
            .HasDatabaseName("ix_coltaxassoc_taxonomyid");

        b.HasIndex(x => x.ColumnId)
            .HasDatabaseName("ix_coltaxassoc_columnid");
    }
}
