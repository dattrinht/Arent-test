namespace HealthApp.Application.Features.BodyRecords;

internal class BodyRecordService : IBodyRecordService
{
    private readonly IBodyRecordRepository _bodyRecordRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IMonthlyAverageCache _monthlyAverageCache;
    private readonly ILogger<BodyRecordService> _logger ;

    public BodyRecordService(
        IBodyRecordRepository bodyRecordRepository,
        IProfileRepository profileRepository,
        IMonthlyAverageCache monthlyAverageCache,
        ILogger<BodyRecordService> logger)
    {
        _bodyRecordRepository = bodyRecordRepository;
        _profileRepository = profileRepository;
        _monthlyAverageCache = monthlyAverageCache;
        _logger = logger;
    }

    public async Task<CreateBodyRecordResponse> CreateAsync(CreateBodyRecordRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var entity = new BodyRecord
        {
            Id = IdGenHelper.CreateId(),
            ProfileId = req.ProfileId,
            Title = req.Title,
            BodyFat = req.BodyFat,
            Weight = req.Weight,
            RecordedAt = req.RecordedAt,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _bodyRecordRepository.SaveAsync(entity, ct);
        _logger.LogInformation("BodyRecord created: {BodyRecordId} for profile {ProfileId}", entity.Id, entity.ProfileId);

        await _monthlyAverageCache.InvalidateAsync(req.ProfileId, ct);

        return new CreateBodyRecordResponse(entity.Id, entity.ProfileId);
    }

    public async Task<BodyRecordSummaryDto?> UpdateAsync(long id, UpdateBodyRecordRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var dto = new BodyRecordSummaryDto(
            id,
            req.Title,
            req.Weight,
            req.BodyFat,
            req.RecordedAt
        );

        var updated = await _bodyRecordRepository.UpdateAsync(dto, ct);
        if (updated is null) return null;

        _logger.LogInformation("BodyRecord updated: {BodyRecordId}", id);

        await _monthlyAverageCache.InvalidateAsync(updated.ProfileId, ct);

        return dto;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _bodyRecordRepository.FindByIdAsync(id, ct);
        if (entity is null) return false;

        var ok = await _bodyRecordRepository.DeleteAsync(id, ct);
        if (ok)
        {
            _logger.LogInformation("BodyRecord soft-deleted: {BodyRecordId}", id);
            await _monthlyAverageCache.InvalidateAsync(entity.ProfileId, ct);
        }

        return ok;
    }

    public async Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>> FetchMonthlyAveragesAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        CancellationToken ct = default
    )
    {
        var _ = await _profileRepository.FindByIdAsync(profileId, ct) ?? throw new InvalidOperationException($"Profile {profileId} does not exist.");

        var cached = await _monthlyAverageCache.GetAsync(profileId, fromMonth, toMonth, ct);
        if (cached is not null)
        {
            _logger.LogInformation("Cache hit for ProfileId={ProfileId}", profileId);
            return cached;
        }

        _logger.LogInformation("Cache miss for ProfileId={ProfileId}", profileId);
        var result = await _bodyRecordRepository.FetchMonthlyAverageAsync(profileId, fromMonth, toMonth, ct);
        await _monthlyAverageCache.SetAsync(profileId, fromMonth, toMonth, result, ct);

        return result;
    }
}
