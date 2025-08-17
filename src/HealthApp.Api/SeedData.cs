using HealthApp.Domain.Models.ExerciseModels;

namespace HealthApp.Api;

public static class SeedData
{
    public static void EnsureSeedData(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HealthAppContext>();
        context.Database.Migrate();

        SeedUserAsync(scope, context).Wait();
        SeedMealsAsync(scope, context).Wait();
        SeedBodyRecordsAsync(scope, context).Wait();
        SeedDiariesAsync(scope, context).Wait();
        SeedExercisesAsync(scope, context).Wait();
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

        var profileResponse = await profileService.CreateProfileAsync(new(authResponse.UserId, "default", "profileResponse", EnumSex.Male));
        Console.WriteLine($"[DbSeeder] Default profileResponse created");
    }

    public static async Task SeedMealsAsync(IServiceScope scope, HealthAppContext context)
    {
        if (await context.Meals.AnyAsync()) return;

        var mealService = scope.ServiceProvider.GetRequiredService<IMealService>();
        var profile = await context.Profiles.FirstAsync();
        var profileId = profile.Id;

        var rnd = new Random();

        var mealNames = new[]
        {
            "Salad", "Pasta", "Chicken Rice", "Fruit Bowl", "Omelette",
            "Soup", "Sandwich", "Burger", "Pizza", "Sushi", "Steak",
            "Ramen", "Tacos", "Curry", "Porridge"
        };

        var mealImageByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Salad"] = "https://images.unsplash.com/photo-1504674900247-0877df9cc836",
            ["Pasta"] = "https://images.unsplash.com/photo-1600891964599-f61ba0e24092",
            ["Chicken Rice"] = "https://images.unsplash.com/photo-1617196039897-efdffb46b4df",
            ["Fruit Bowl"] = "https://images.unsplash.com/photo-1572441710534-680d2a8b9a2b",
            ["Omelette"] = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c",
            ["Soup"] = "https://images.unsplash.com/photo-1553621042-f6e147245754",
            ["Sandwich"] = "https://images.unsplash.com/photo-1568605114967-8130f3a36994",
            ["Burger"] = "https://images.unsplash.com/photo-1550547660-d9450f859349",
            ["Pizza"] = "https://images.unsplash.com/photo-1594007654729-407eedc4be65",
            ["Sushi"] = "https://images.unsplash.com/photo-1546069901-5b3a3b4f1e7e",
            ["Steak"] = "https://images.unsplash.com/photo-1553163147-622ab57be1c7",
            ["Ramen"] = "https://images.unsplash.com/photo-1543352632-1735d3201a10",
            ["Tacos"] = "https://images.unsplash.com/photo-1543339308-43f2d7b3c3c7",
            ["Curry"] = "https://images.unsplash.com/photo-1604908177079-4f32a3eaad47",
            ["Porridge"] = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd"
        };

        static string GetImageFor(string name, IReadOnlyDictionary<string, string> map) =>
            map.TryGetValue(name, out var url)
            ? url
            : "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe";

        int count = 50;
        for (int i = 0; i < count; i++)
        {
            var name = mealNames[rnd.Next(mealNames.Length)];
            var image = GetImageFor(name, mealImageByName);

            var type = name switch
            {
                "Omelette" or "Porridge" or "Fruit Bowl" => EnumMealType.Breakfast,
                "Salad" or "Sandwich" => EnumMealType.Lunch,
                "Pizza" or "Sushi" or "Steak" or "Curry" => EnumMealType.Dinner,
                _ => EnumMealType.Other
            };

            var doneAt = DateTime.UtcNow
                .AddDays(-rnd.Next(0, 14))
                .AddHours(rnd.Next(0, 24))
                .AddMinutes(rnd.Next(0, 60));

            await mealService.CreateAsync(new CreateMealRequest(
                profileId,
                name,
                type,
                image,
                doneAt
            ));
        }

        Console.WriteLine($"[DbSeeder] {count} random meals seeded for profile {profileId}");
    }

    public static async Task SeedBodyRecordsAsync(IServiceScope scope, HealthAppContext context)
    {
        if (await context.BodyRecords.AnyAsync()) return;

        var service = scope.ServiceProvider.GetRequiredService<IBodyRecordService>();
        var profile = await context.Profiles.FirstAsync();
        var profileId = profile.Id;

        var rnd = new Random();

        var count = 50;
        for (int i = 0; i < count; i++)
        {
            var title = "demo";
            var weight = (float)Math.Round(rnd.NextDouble() * 10, 1);
            var bodyFat = (float)Math.Round(rnd.NextDouble() * 10, 1);

            var dt = DateTime.UtcNow.Date.AddDays(-rnd.Next(0, 360));
            var recordedAt = DateOnly.FromDateTime(dt);

            await service.CreateAsync(new CreateBodyRecordRequest(
                profileId,
                title,
                weight,
                bodyFat,
                recordedAt
            ));
        }

        Console.WriteLine($"[DbSeeder] {count} body records seeded for profile {profileId}");
    }

    public static async Task SeedDiariesAsync(IServiceScope scope, HealthAppContext context)
    {
        if (await context.Diaries.AnyAsync()) return;

        var diaryService = scope.ServiceProvider.GetRequiredService<IDiaryService>();
        var profile = await context.Profiles.FirstAsync();
        var profileId = profile.Id;

        var rnd = new Random();

        var titles = new[]
        {
            "Morning Reflection", "Workout Log", "Daily Gratitude",
            "Study Notes", "Meal Thoughts", "Evening Recap", "Mood Check"
        };

        var loremWords = new[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetur",
            "adipiscing", "elit", "integer", "nec", "odio", "praesent",
            "libero", "sed", "cursus", "ante", "dapibus", "diam"
        };

        string GenerateLorem(int wordCount)
        {
            return string.Join(" ", Enumerable.Range(0, wordCount)
                .Select(_ => loremWords[rnd.Next(loremWords.Length)]));
        }

        int count = 50;
        for (int i = 0; i < count; i++)
        {
            var title = titles[rnd.Next(titles.Length)];

            var content = $"{title}: {GenerateLorem(rnd.Next(30, 80))}.";

            var dt = DateTime.UtcNow
                .AddDays(-rnd.Next(0, 180))
                .AddHours(rnd.Next(0, 24))
                .AddMinutes(rnd.Next(0, 60));

            await diaryService.CreateAsync(new CreateDiaryRequest(
                ProfileId: profileId,
                Title: title,
                Content: content,
                WrittenAt: dt
            ));
        }

        Console.WriteLine($"[DbSeeder] {count} diaries seeded for profile {profileId}");
    }

    public static async Task SeedExercisesAsync(IServiceScope scope, HealthAppContext context)
    {
        if (await context.Exercises.AnyAsync()) return;

        var service = scope.ServiceProvider.GetRequiredService<IExerciseService>();
        var profile = await context.Profiles.FirstAsync();
        var profileId = profile.Id;

        var rnd = new Random();

        var count = 50;
        for (int i = 0; i < count; i++)
        {
            var name = "demo";
            var type = (EnumExerciseType)rnd.Next(0, 4);
            var status = (EnumExerciseStatus)rnd.Next(0, 4);
            var durationSec = rnd.Next(300, 3600);
            var calories = rnd.Next(50, 200);

            var dt = DateTime.UtcNow.Date.AddDays(-rnd.Next(0, 180));

            await service.CreateAsync(new CreateExerciseRequest(
                profileId,
                name,
                type,
                status,
                durationSec,
                calories,
                dt
            ));
        }

        Console.WriteLine($"[DbSeeder] {count} exercises seeded for profile {profileId}");
    }
}
