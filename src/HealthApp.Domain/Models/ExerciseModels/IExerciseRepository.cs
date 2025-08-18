namespace HealthApp.Domain.Models.ExerciseModels;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<Exercise> SaveAsync(Exercise exercise, CancellationToken ct = default);
    Task<Exercise?> UpdateAsync(ExerciseSummaryDto dto, CancellationToken ct = default);
    Task<Exercise?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<TResult> Items, int TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<Exercise, TResult> spec,
        CancellationToken ct = default
    );
    Task<ExerciseAchievementDto> GetAchievementAsync(
        long profileId,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        CancellationToken ct = default
    );
}
