namespace HealthApp.Application.Features.Profiles.Models;

public record CreateProfileRequest(long UserId, string FisrtName, string LastName, EnumSex Sex);
