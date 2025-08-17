namespace HealthApp.Domain.Models.DiaryModels;

public interface IDiaryRepository
{
    Task<Diary> SaveAsync(Diary meal, CancellationToken ct = default);
    Task<Diary?> UpdateAsync(Diary diary, CancellationToken ct = default);
    Task<Diary?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(ISimplePagingSpecification<Diary, TResult> spec, CancellationToken ct = default);
}
