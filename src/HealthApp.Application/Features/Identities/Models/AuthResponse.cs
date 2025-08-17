namespace HealthApp.Application.Features.Identities.Models;

public sealed record AuthResponse(long UserId, string Email, string AccessToken);