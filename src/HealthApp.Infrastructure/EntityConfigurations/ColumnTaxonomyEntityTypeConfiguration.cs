namespace HealthApp.Infrastructure.EntityConfigurations;

internal sealed class ColumnTaxonomyEntityTypeConfiguration : IEntityTypeConfiguration<ColumnTaxonomy>
{
    public void Configure(EntityTypeBuilder<ColumnTaxonomy> builder)
    {
        builder.ToTable("column_taxonomies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Type)
            .HasConversion<short>()
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasMany(x => x.ColumnTaxonomies)
            .WithOne(a => a.Taxonomy)
            .HasForeignKey(a => a.TaxonomyId);

        builder.HasIndex(x => new { x.Type, x.Name })
            .IsUnique()
            .HasDatabaseName("ux_coltax_type_name");
    }
}
