namespace HealthApp.Application.Features.Meals;

public interface IMealService
{
    Task<CreateMealResponse> CreateAsync(CreateMealRequest req, CancellationToken ct = default);
    Task<(IReadOnlyList<MealSummaryDto> Items, long TotalCount)> FetchByProfileAsync(
        long profileId,
        EnumMealType? type,
        int page,
        int pageSize,
        CancellationToken ct = default
    );
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
}
