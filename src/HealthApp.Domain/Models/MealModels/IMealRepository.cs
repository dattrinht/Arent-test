namespace HealthApp.Domain.Models.MealModels;

public interface IMealRepository : IRepository<Meal>
{
    Task<Meal> SaveAsync(Meal meal, CancellationToken ct = default);
    Task<Meal?> UpdateAsync(MealSummaryDto dto, CancellationToken ct = default);
    Task<Meal?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(ISimplePagingSpecification<Meal, TResult> spec, CancellationToken ct = default);
}
