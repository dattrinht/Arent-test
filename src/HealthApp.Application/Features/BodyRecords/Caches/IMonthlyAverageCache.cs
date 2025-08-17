namespace HealthApp.Application.Features.BodyRecords.Caches;

public interface IMonthlyAverageCache
{
    Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>?> GetAsync(
        long profileId, DateOnly fromMonth, DateOnly toMonth, CancellationToken ct = default);
    Task SetAsync(
        long profileId, DateOnly fromMonth, DateOnly toMonth,
        IReadOnlyList<BodyRecordMonthlyAggregateDto> payload,
        CancellationToken ct = default);
    Task InvalidateAsync(long profileId, CancellationToken ct = default);
}
