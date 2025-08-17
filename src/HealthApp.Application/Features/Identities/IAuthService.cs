namespace HealthApp.Application.Features.Identities;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(AuthRequest req, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(AuthRequest req, CancellationToken ct = default);
}