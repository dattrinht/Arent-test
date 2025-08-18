namespace HealthApp.Domain.Models.DiaryModels;

public interface IDiaryRepository : IRepository<Diary>
{
    Task<Diary> SaveAsync(Diary meal, CancellationToken ct = default);
    Task<Diary?> UpdateAsync(DiaryDetailDto dto, CancellationToken ct = default);
    Task<Diary?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<TResult> Items, int TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<Diary, TResult> spec,
        CancellationToken ct = default
    );
}
