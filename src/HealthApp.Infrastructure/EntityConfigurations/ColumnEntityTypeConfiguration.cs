namespace HealthApp.Infrastructure.EntityConfigurations;

internal sealed class ColumnEntityTypeConfiguration : IEntityTypeConfiguration<Column>
{
    public void Configure(EntityTypeBuilder<Column> builder)
    {
        builder.ToTable("columns");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasMany(x => x.ColumnTaxonomies)
            .WithOne(ct => ct.Column)
            .HasForeignKey(ct => ct.ColumnId);

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("ux_columns_slug");

        builder.HasIndex(x => new { x.IsPublished, x.IsDeleted, x.PublishedAt })
            .HasDatabaseName("ix_columns_published_isdeleted_publishedat");
    }
}
