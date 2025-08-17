namespace HealthApp.Infrastructure.EntityConfigurations;

internal class MealEntityTypeConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.ToTable("meals");

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.ProfileId)
            .IsRequired();

        builder.Property(m => m.Name)
            .HasMaxLength(128);

        builder.Property(m => m.Type)
            .HasConversion<short>()
            .IsRequired();

        builder.Property(m => m.Image)
            .HasMaxLength(256);

        builder.Property(m => m.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(m => !m.IsDeleted);

        builder.HasIndex(m => new { m.ProfileId, m.IsDeleted, m.DoneAt })
            .HasDatabaseName("ix_meal_profileid_isdeleted_doneat");
    }
}
