namespace HealthApp.Infrastructure.Repositories;

public class MealRepository : IMealRepository
{
    private readonly HealthAppContext _dbContext;

    public MealRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Meal> SaveAsync(Meal meal, CancellationToken ct = default)
    {
        _dbContext.Meals.Add(meal);
        await _dbContext.SaveChangesAsync(ct);
        return meal;
    }

    public async Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(ISimplePagingSpecification<Meal, TResult> spec, CancellationToken ct = default)
    {
        IQueryable<Meal> baseQuery = _dbContext.Meals;

        var filtered = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.LongCountAsync(ct);
        var items = await filtered.ToListAsync(ct);

        return (items, total);
    }

    public async Task<Meal?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        return await _dbContext.Meals
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Meals.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (entity is null) return false;
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }
}
