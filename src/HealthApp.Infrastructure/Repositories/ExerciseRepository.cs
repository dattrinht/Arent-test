namespace HealthApp.Infrastructure.Repositories;

internal class ExerciseRepository : IExerciseRepository
{
    private readonly HealthAppContext _dbContext;

    public ExerciseRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Exercise> SaveAsync(Exercise exercise, CancellationToken ct = default)
    {
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync(ct);
        return exercise;
    }

    public async Task<Exercise?> UpdateAsync(ExerciseSummaryDto dto, CancellationToken ct = default)
    {
        var entity = await _dbContext.Exercises
            .FirstOrDefaultAsync(d => d.Id == dto.Id, ct);
        if (entity is null) return null;

        entity.Name = dto.Name?.Trim() ?? string.Empty;
        entity.DurationSec = dto.DurationSec;
        entity.CaloriesBurned = dto.CaloriesBurned;
        entity.Status = dto.Status;
        entity.Type = dto.Type;
        entity.FinishedAt = dto.FinishedAt;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<Exercise, TResult> spec,
        CancellationToken ct = default)
    {
        IQueryable<Exercise> baseQuery = _dbContext.Exercises;

        var projected = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.LongCountAsync(ct);
        var items = await projected.ToListAsync(ct);

        return (items, total);
    }

    public async Task<Exercise?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        return await _dbContext.Exercises
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Exercises.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null) return false;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }
}
