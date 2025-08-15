namespace HealthApp.Domain.Models.AuthModels;

public interface IUserRepository : IRepository<User>
{
    Task<User> CreateAsync(User user);
    Task<User?> FindByEmailAsync(string email);
}
