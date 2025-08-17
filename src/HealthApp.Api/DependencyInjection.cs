using Microsoft.OpenApi.Any;

namespace HealthApp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HealthApp API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your token."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.MapType<AuthRequest>(() => new OpenApiSchema
            {
                Type = "object",
                Example = new OpenApiObject
                {
                    ["email"] = new OpenApiString("admin@healthapp.com"),
                    ["password"] = new OpenApiString("Admin@123"),
                }
            });
        });

        return services;
    }
}