namespace HealthApp.Application.Auth.Models;

public sealed record AuthResponse(long UserId, string Email, string AccessToken);