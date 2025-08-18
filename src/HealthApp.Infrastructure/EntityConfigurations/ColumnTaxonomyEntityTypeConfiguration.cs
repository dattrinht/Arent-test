namespace HealthApp.Infrastructure.EntityConfigurations;

internal sealed class ColumnTaxonomyEntityTypeConfiguration : IEntityTypeConfiguration<ColumnTaxonomy>
{
    public void Configure(EntityTypeBuilder<ColumnTaxonomy> b)
    {
        b.ToTable("column_taxonomies");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedNever();

        b.Property(x => x.Type)
            .HasConversion<short>()
            .IsRequired();

        b.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasMany(x => x.ColumnTaxonomies)
            .WithOne(a => a.Taxonomy)
            .HasForeignKey(a => a.TaxonomyId);

        b.HasIndex(x => new { x.Type, x.Name })
            .IsUnique()
            .HasDatabaseName("ux_coltax_type_name");
    }
}
