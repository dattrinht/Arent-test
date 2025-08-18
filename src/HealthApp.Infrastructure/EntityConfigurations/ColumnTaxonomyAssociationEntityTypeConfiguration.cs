namespace HealthApp.Infrastructure.EntityConfigurations;

internal sealed class ColumnTaxonomyAssociationEntityTypeConfiguration : IEntityTypeConfiguration<ColumnTaxonomyAssociation>
{
    public void Configure(EntityTypeBuilder<ColumnTaxonomyAssociation> builder)
    {
        builder.ToTable("column_taxonomy_associations");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Column)
            .WithMany(c => c.ColumnTaxonomies)
            .HasForeignKey(x => x.ColumnId);

        builder.HasOne(x => x.Taxonomy)
            .WithMany(t => t.ColumnTaxonomies)
            .HasForeignKey(x => x.TaxonomyId);

        builder.HasQueryFilter(a => !a.Column.IsDeleted && !a.Taxonomy.IsDeleted);

        builder.HasIndex(x => new { x.ColumnId, x.TaxonomyId })
            .IsUnique()
            .HasDatabaseName("ux_column_taxonomy_association");

        builder.HasIndex(x => x.TaxonomyId)
            .HasDatabaseName("ix_coltaxassoc_taxonomyid");

        builder.HasIndex(x => x.ColumnId)
            .HasDatabaseName("ix_coltaxassoc_columnid");
    }
}
