namespace HealthApp.Infrastructure.Repositories;

internal class DiaryRepository : IDiaryRepository
{
    private readonly HealthAppContext _dbContext;

    public DiaryRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Diary> SaveAsync(Diary diary, CancellationToken ct = default)
    {
        diary.Preview = GeneratePreview(diary.Content);
        _dbContext.Diaries.Add(diary);
        await _dbContext.SaveChangesAsync(ct);
        return diary;
    }

    public async Task<Diary?> UpdateAsync(DiaryDetailDto dto, CancellationToken ct = default)
    {
        var entity = await _dbContext.Diaries
            .FirstOrDefaultAsync(d => d.Id == dto.Id, ct);
        if (entity is null) return null;

        entity.Title = dto.Title?.Trim() ?? string.Empty;
        entity.Content = dto.Content ?? string.Empty;
        entity.Preview = GeneratePreview(entity.Content);
        entity.WrittenAt = dto.WrittenAt;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<Diary, TResult> spec,
        CancellationToken ct = default
    )
    {
        IQueryable<Diary> baseQuery = _dbContext.Diaries;

        var projected = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.LongCountAsync(ct);
        var items = await projected.ToListAsync(ct);

        return (items, total);
    }

    public async Task<Diary?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        return await _dbContext.Diaries
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Diaries.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null) return false;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    private static string GeneratePreview(string? content, int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(content)) return string.Empty;

        var trimmed = content.Trim();
        if (trimmed.Length <= maxLength) return trimmed;

        return trimmed[..maxLength] + "...";
    }
}
