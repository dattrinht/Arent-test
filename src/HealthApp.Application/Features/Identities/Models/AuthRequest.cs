namespace HealthApp.Application.Features.Identities.Models;

public sealed record AuthRequest(string Email, string Password);