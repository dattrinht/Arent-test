namespace HealthApp.Infrastructure.EntityConfigurations;

internal class DiaryEntityTypeConfiguration : IEntityTypeConfiguration<Diary>
{
    public void Configure(EntityTypeBuilder<Diary> builder)
    {
        builder.ToTable("diaries");

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ProfileId)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => new { x.ProfileId, x.IsDeleted, x.WrittenAt })
            .HasDatabaseName("ix_bodyrecord_profileid_isdeleted_writtenat");
    }
}
