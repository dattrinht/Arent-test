using HealthApp.Domain.Models.ProfileModels;

namespace HealthApp.Application.Features.Meals;

internal class MealService : IMealService
{
    private readonly IMealRepository _mealRepository;
    private readonly IProfileRepository _profileRepository;

    public MealService(IMealRepository mealRepository, IProfileRepository profileRepository)
    {
        _mealRepository = mealRepository;
        _profileRepository = profileRepository;
    }

    public async Task<CreateMealResponse> CreateAsync(CreateMealRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var meal = new Meal
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

        await _mealRepository.SaveAsync(meal, ct);
        return new CreateMealResponse(meal.Id, meal.ProfileId);
    }

    public async Task<(IReadOnlyList<MealSummaryDto> Items, long TotalCount)> FetchByProfileAsync(long profileId, int page, int pageSize, CancellationToken ct = default)
    {
        var spec = new MealsByProfileIdPagingSpec(profileId, page, pageSize);
        return await _mealRepository.FetchByProfileIdAsync(spec, ct);
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        return await _mealRepository.DeleteAsync(id, ct);
    }
}
