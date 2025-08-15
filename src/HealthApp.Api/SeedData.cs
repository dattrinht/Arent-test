using Microsoft.EntityFrameworkCore;

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
        if (!await context.Users.AnyAsync())
        {
            var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

            var email = "admin@healthapp.com";
            var password = "Admin@123";

            await authService.RegisterAsync(new(email, password));

            Console.WriteLine($"[DbSeeder] Default admin user created: {email}");
        }
    }
}
