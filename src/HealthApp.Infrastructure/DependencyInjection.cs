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

        services.AddStackExchangeRedisCache(options =>
        {
            var cs = configuration.GetConnectionString("Redis")
                     ?? throw new InvalidOperationException("Missing connection string 'Redis'");
            options.Configuration = cs;
            options.InstanceName = "healthapp:";
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IMealRepository, MealRepository>();
        services.AddScoped<IBodyRecordRepository, BodyRecordRepository>();

        return services;
    }
}