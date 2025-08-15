namespace HealthApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HealthAppContext>(opt =>
        {
            var cs = configuration.GetConnectionString("Postgres")
                     ?? throw new InvalidOperationException("Missing connection string 'Postgres'");
            opt.UseNpgsql(cs);
        });

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var cs = configuration.GetConnectionString("Redis")
                     ?? throw new InvalidOperationException("Missing connection string 'Redis'");
            return ConnectionMultiplexer.Connect(cs);
        });

        services.AddSingleton<IDistributedLockFactory>(sp =>
        {
            var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            var redlockMultiplexers = new List<RedLockMultiplexer>
            {
                new(multiplexer)
            };
            return RedLockFactory.Create(redlockMultiplexers);
        });

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}