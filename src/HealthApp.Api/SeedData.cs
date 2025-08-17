namespace HealthApp.Api;

public static class SeedData
{
    public static void EnsureSeedData(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthAppContext>();
        context.Database.Migrate();

        SeedUserAsync(scope, context).Wait();
    }

    public static async Task SeedUserAsync(IServiceScope scope, HealthAppContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var profileService = scope.ServiceProvider.GetRequiredService<IProfileService>();

        var email = "admin@healthapp.com";
        var password = "Admin@123";

        var authResponse = await authService.RegisterAsync(new(email, password));
        Console.WriteLine($"[DbSeeder] Default admin user created: {email}");

        await profileService.CreateProfileAsync(new(authResponse.UserId, "default", "profile", EnumSex.Male));
        Console.WriteLine($"[DbSeeder] Default profile created");
    }
}
