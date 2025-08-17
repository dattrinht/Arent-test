namespace HealthApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HealthAppContext _dbContext;

    public UserRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> SaveAsync(User user, CancellationToken ct = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(ct);

        return user;
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email, ct);

        return user;
    }
}
