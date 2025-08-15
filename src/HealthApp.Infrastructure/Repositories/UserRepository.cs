namespace HealthApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HealthAppContext _dbContext;

    public UserRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email);

        return user;
    }
}
