namespace HealthApp.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly HealthAppContext _dbContext;

    public ProfileRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Profile> CreateAsync(Profile profile, CancellationToken ct = default)
    {
        _dbContext.Profiles.Add(profile);
        await _dbContext.SaveChangesAsync(ct);
        return profile;
    }

    public Task<Profile?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        return _dbContext.Profiles
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<(IReadOnlyList<T> Items, long TotalCount)> FetchByUserIdAsync<T>(ISimplePagingSpecification<Profile, T> spec, CancellationToken ct = default)
    {
        IQueryable<Profile> baseQuery = _dbContext.Profiles;

        var filtered = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.LongCountAsync(ct);
        var items = await filtered.ToListAsync(ct);

        return (items, total);
    }
}
