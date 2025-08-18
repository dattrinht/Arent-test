namespace HealthApp.Application.Features.Meals;

internal class MealService : IMealService
{
    private readonly IMealRepository _mealRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<MealService> _logger;

    public MealService(
        IMealRepository mealRepository,
        IProfileRepository profileRepository,
        ILogger<MealService> logger
    )
    {
        _mealRepository = mealRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateMealResponse> CreateAsync(CreateMealRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var entity = new Meal
        {
            Id = IdGenHelper.CreateId(),
            ProfileId = req.ProfileId,
            Name = req.Name.Trim(),
            Type = req.Type,
            Image = string.IsNullOrWhiteSpace(req.Image) ? null : req.Image.Trim(),
            DoneAt = req.DoneAt,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _mealRepository.SaveAsync(entity, ct);
        _logger.LogInformation("Meal created: {MealId} for profile {ProfileId}", entity.Id, entity.ProfileId);

        return new CreateMealResponse(entity.Id, entity.ProfileId);
    }

    public async Task<(IReadOnlyList<MealSummaryDto> Items, int TotalCount)> FetchByProfileAsync(
        long profileId,
        EnumMealType? type,
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var spec = new MealsByProfileIdPagingSpec(profileId, type, page, pageSize);
        return await _mealRepository.FetchByProfileIdAsync(spec, ct);
    }

    public async Task<MealSummaryDto?> UpdateAsync(long id, UpdateMealRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var dto = new MealSummaryDto(
            id,
            default,
            req.Name,
            req.Type,
            req.Image,
            req.DoneAt
        );

        var updated = await _mealRepository.UpdateAsync(dto, ct);
        if (updated is null) return null;

        _logger.LogInformation("Meal updated: {MealId}", id);

        return dto;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var ok = await _mealRepository.DeleteAsync(id, ct);
        if (ok) _logger.LogInformation("Meal soft-deleted: {MealId}", id);
        return ok;
    }
}
