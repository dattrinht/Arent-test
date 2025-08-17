namespace HealthApp.Infrastructure.EntityConfigurations;

internal class ExerciseEntityTypeConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ProfileId)
            .IsRequired();

        builder.Property(p => p.Type)
            .HasConversion<short>();

        builder.Property(p => p.Status)
            .HasConversion<short>();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => new { x.ProfileId, x.IsDeleted, x.FinishedAt })
            .HasDatabaseName("ix_bodyrecord_profileid_isdeleted_finishedat");
    }
}
