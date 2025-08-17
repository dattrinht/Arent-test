namespace HealthApp.Domain.Models.IdentityModels;

public interface IUserRepository : IRepository<User>
{
    Task<User> CreateAsync(User user, CancellationToken ct = default);
    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
}
