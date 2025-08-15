namespace HealthApp.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'HealthApp.Infrastructure' project directory:
///
/// dotnet ef migrations add --startup-project HealthApp.Api --context HealthAppContext [migration-name]
/// </remarks>
public class HealthAppContext : DbContext
{
    public HealthAppContext(DbContextOptions<HealthAppContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("healthapp");
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}
