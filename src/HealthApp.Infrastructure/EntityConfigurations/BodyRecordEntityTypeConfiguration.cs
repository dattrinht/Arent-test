namespace HealthApp.Infrastructure.EntityConfigurations;

internal class BodyRecordEntityTypeConfiguration : IEntityTypeConfiguration<BodyRecord>
{
    public void Configure(EntityTypeBuilder<BodyRecord> builder)
    {
        builder.ToTable("body_records");

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ProfileId)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => new { x.ProfileId, x.IsDeleted, x.RecordedAt })
            .HasDatabaseName("ix_bodyrecord_profileid_isdeleted_recordedat");
    }
}
