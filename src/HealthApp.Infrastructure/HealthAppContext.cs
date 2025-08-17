namespace HealthApp.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the root project directory:
///
/// dotnet ef migrations add [migration-nam] -p src/HealthApp.Infrastructure -s src/HealthApp.Api -c HealthAppContext -o Migrations
/// </remarks>
public class HealthAppContext : DbContext
{
    public HealthAppContext(DbContextOptions<HealthAppContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("healthapp");
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProfileEntityTypeConfiguration());
    }
}
