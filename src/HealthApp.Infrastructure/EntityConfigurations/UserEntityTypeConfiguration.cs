namespace HealthApp.Infrastructure.EntityConfigurations;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
