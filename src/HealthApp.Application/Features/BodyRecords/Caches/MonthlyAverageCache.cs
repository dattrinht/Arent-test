namespace HealthApp.Application.Features.BodyRecords.Caches;

internal class MonthlyAverageCache : IMonthlyAverageCache
{
    private readonly IDistributedCache _distributedCache;

    public MonthlyAverageCache(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    private static string VersionKey(long profileId) => $"bodyrecord:ver:{profileId}";
    private static string CacheKey(long profileId, string ver, DateOnly from, DateOnly to)
        => $"bodyrecord:avg:{profileId}:{ver}:{from:yyyyMM}:{to:yyyyMM}";

    public async Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>?> GetAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        CancellationToken ct = default
    )
    {
        var ver = await GetVersionAsync(profileId, ct);
        var key = CacheKey(profileId, ver, Normalize(fromMonth), Normalize(toMonth));

        var bytes = await _distributedCache.GetAsync(key, ct);
        if (bytes is null) return null;

        return JsonSerializer.Deserialize<IReadOnlyList<BodyRecordMonthlyAggregateDto>>(bytes);
    }

    public async Task SetAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        IReadOnlyList<BodyRecordMonthlyAggregateDto> payload,
        CancellationToken ct = default
    )
    {
        var ver = await GetVersionAsync(profileId, ct);
        var key = CacheKey(profileId, ver, Normalize(fromMonth), Normalize(toMonth));

        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        await _distributedCache.SetAsync(key, bytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(5)
        }, ct);
    }

    public async Task InvalidateAsync(long profileId, CancellationToken ct = default)
    {
        var newVer = Guid.NewGuid().ToString("n");
        await _distributedCache.SetStringAsync(VersionKey(profileId), newVer, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
        }, ct);
    }

    private async Task<string> GetVersionAsync(long profileId, CancellationToken ct)
    {
        var version = await _distributedCache.GetStringAsync(VersionKey(profileId), ct);
        if (!string.IsNullOrWhiteSpace(version)) return version!;
        var newVer = Guid.NewGuid().ToString("n");
        await _distributedCache.SetStringAsync(VersionKey(profileId), newVer, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
        }, ct);
        return newVer;
    }

    private static DateOnly Normalize(DateOnly month) => new(month.Year, month.Month, 1);
}
