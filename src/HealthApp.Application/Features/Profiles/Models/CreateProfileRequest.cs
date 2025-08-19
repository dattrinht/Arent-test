namespace HealthApp.Application.Features.Profiles.Models;

public sealed record CreateProfileRequest(
    [Required] long UserId,
    [Required, StringLength(100, MinimumLength = 1)] string FirstName,
    [Required, StringLength(100, MinimumLength = 1)] string LastName,
    [Required]  EnumSex Sex
);
