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

    public async Task<(IReadOnlyList<TResult> Items, int TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<Exercise, TResult> spec,
        CancellationToken ct = default
    )
    {
        IQueryable<Exercise> baseQuery = _dbContext.Exercises;

        var projected = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.CountAsync(ct);
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

    public async Task<ExerciseAchievementDto> GetAchievementAsync(
        long profileId,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        CancellationToken ct = default
    )
    {
        // if no fromDate provided --> from = today
        var actualFromDate = fromDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var from = DateTime.SpecifyKind(actualFromDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);

        // if no toDate provided --> to = end of fromDate
        var actualToDate = toDate ?? actualFromDate;
        var to = DateTime.SpecifyKind(actualToDate.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);

        var query = _dbContext.Exercises
            .Where(x => x.ProfileId == profileId)
            .Where(e => e.FinishedAt >= from && e.FinishedAt <= to);

        var total = await query.CountAsync(ct);
        var completed = await query.CountAsync(e => e.Status == EnumExerciseStatus.Completed, ct);

        return new ExerciseAchievementDto(total, completed);
    }
}
