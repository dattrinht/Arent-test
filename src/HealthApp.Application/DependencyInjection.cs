namespace HealthApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IMealService, MealService>();
        services.AddScoped<IBodyRecordService, BodyRecordService>();
        services.AddScoped<IMonthlyAverageCache, MonthlyAverageCache>();
        services.AddScoped<IDiaryService, DiaryService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IColumnTaxonomyService, ColumnTaxonomyService>();
        services.AddScoped<IColumnService, ColumnService>();

        return services;
    }
}