namespace HealthApp.Application.Features.Diaries;

internal class DiaryService : IDiaryService
{
    private readonly IDiaryRepository _diaryRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<DiaryService> _logger;

    public DiaryService(
        IDiaryRepository diaryRepository,
        IProfileRepository profileRepository,
        ILogger<DiaryService> logger)
    {
        _diaryRepository = diaryRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateDiaryResponse> CreateAsync(CreateDiaryRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var entity = new Diary
        {
            Id = IdGenHelper.CreateId(),
            ProfileId = req.ProfileId,
            Title = req.Title.Trim(),
            Content = req.Content,
            WrittenAt = req.WrittenAt,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _diaryRepository.SaveAsync(entity, ct);
        _logger.LogInformation("Diary created: {DiaryId} for profile {ProfileId}", entity.Id, entity.ProfileId);

        return new CreateDiaryResponse(entity.Id, entity.ProfileId);
    }

    public async Task<DiaryDetailDto?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        var entity = await _diaryRepository.FindByIdAsync(id, ct);
        return entity is null ? null : new DiaryDetailDto(
            entity.Id,
            entity.ProfileId,
            entity.Title,
            entity.Content,
            entity.Preview,
            entity.WrittenAt,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }

    public async Task<(IReadOnlyList<DiarySummaryDto> Items, long TotalCount)> FetchByProfileAsync(
        long profileId,
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var spec = new DiariesByProfileIdPagingSpec(profileId, page, pageSize);
        return await _diaryRepository.FetchByProfileIdAsync(spec, ct);
    }

    public async Task<DiaryDetailDto?> UpdateAsync(long id, UpdateDiaryRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var existing = await _diaryRepository.FindByIdAsync(id, ct);
        if (existing is null) return null;

        existing.Title = req.Title.Trim();
        existing.Content = req.Content;
        existing.WrittenAt = req.WrittenAt;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _diaryRepository.UpdateAsync(existing, ct);
        if (updated is null) return null;

        _logger.LogInformation("Diary updated: {DiaryId}", id);

        return new DiaryDetailDto(
            updated.Id,
            updated.ProfileId,
            updated.Title,
            updated.Content,
            updated.Preview,
            updated.WrittenAt,
            updated.CreatedAt,
            updated.UpdatedAt
        );
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var ok = await _diaryRepository.DeleteAsync(id, ct);
        if (ok) _logger.LogInformation("Diary soft-deleted: {DiaryId}", id);
        return ok;
    }
}
