namespace HealthApp.Application.Features.Exercises;

public interface IExerciseService
{
    Task<CreateExerciseResponse> CreateAsync(CreateExerciseRequest req, CancellationToken ct = default);
    Task<(IReadOnlyList<ExerciseSummaryDto> Items, long TotalCount)> FetchByProfileAsync(long profileId, DateOnly? byDate, int page, int pageSize, CancellationToken ct = default);
    Task<ExerciseSummaryDto?> UpdateAsync(long id, UpdateExerciseRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<ExerciseAchievementDto> GetAchievementByDateAsync(long profileId, DateOnly day, CancellationToken ct = default);
}
