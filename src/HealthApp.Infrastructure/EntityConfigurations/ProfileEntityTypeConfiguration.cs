namespace HealthApp.Infrastructure.EntityConfigurations;

internal class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Sex)
            .HasConversion<short>();

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
