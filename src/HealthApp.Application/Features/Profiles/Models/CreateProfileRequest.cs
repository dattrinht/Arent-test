namespace HealthApp.Application.Features.Profiles.Models;

public sealed record CreateProfileRequest(
    [property: Required] long UserId,
    [property: Required, StringLength(100, MinimumLength = 1)] string FirstName,
    [property: Required, StringLength(100, MinimumLength = 1)] string LastName,
    [property: Required]  EnumSex Sex
);
