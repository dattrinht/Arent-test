using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new Int64ToStringConverter());
    opt.JsonSerializerOptions.Converters.Add(new NullableInt64ToStringConverter());
    opt.JsonSerializerOptions.NumberHandling =
        JsonNumberHandling.AllowReadingFromString;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "healthapp",
            ValidAudience = "healthapp",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DEMO_ONLY_SUPER_SECRET_256BIT_KEY_DEMO_ONLY")),
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //This is done for development ease but shouldn't be here in production
    app.EnsureSeedData();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
