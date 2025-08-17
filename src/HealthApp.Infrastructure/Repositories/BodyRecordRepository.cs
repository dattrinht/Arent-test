namespace HealthApp.Infrastructure.Repositories;

internal class BodyRecordRepository : IBodyRecordRepository
{
    private readonly HealthAppContext _dbContext;

    public BodyRecordRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BodyRecord> SaveAsync(BodyRecord bodyRecord, CancellationToken ct = default)
    {
        _dbContext.BodyRecords.Add(bodyRecord);
        await _dbContext.SaveChangesAsync(ct);
        return bodyRecord;
    }

    public async Task<BodyRecord?> UpdateAsync(BodyRecordSummaryDto dto, CancellationToken ct = default)
    {
        var entity = await _dbContext.BodyRecords
            .FirstOrDefaultAsync(d => d.Id == dto.Id, ct);
        if (entity is null) return null;

        entity.Title = dto.Title?.Trim() ?? string.Empty;
        entity.Weight = dto.Weight;
        entity.BodyFat = dto.BodyFat;
        entity.RecordedAt = dto.RecordedAt;
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<BodyRecord?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        return await _dbContext.BodyRecords
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _dbContext.BodyRecords.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (entity is null) return false;
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>> FetchMonthlyAverageAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        CancellationToken ct = default
    )
    {
        var start = new DateOnly(fromMonth.Year, fromMonth.Month, 1);
        var end = new DateOnly(toMonth.Year, toMonth.Month, 1).AddMonths(1);

        var records = await _dbContext.BodyRecords
            .Where(b => b.ProfileId == profileId
                && b.RecordedAt >= start
                && b.RecordedAt < end
            )
            .AsNoTracking()
            .Select(b => new { b.RecordedAt, b.Weight, b.BodyFat })
            .ToListAsync(ct);

        var result = records
            .GroupBy(x => new { x.RecordedAt.Year, x.RecordedAt.Month })
            .Select(g => new BodyRecordMonthlyAggregateDto(
                Year: g.Key.Year,
                Month: g.Key.Month,
                AverageWeight: (float)g.Average(x => x.Weight),
                AverageBodyFat: (float)g.Average(x => x.BodyFat)
            ))
            .OrderBy(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList();

        return result;
    }
}
