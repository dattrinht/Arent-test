namespace HealthApp.Application.Features.Diaries;

public interface IDiaryService
{
    Task<CreateDiaryResponse> CreateAsync(CreateDiaryRequest req, CancellationToken ct = default);
    Task<DiaryDetailDto?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<DiarySummaryDto> Items, long TotalCount)> FetchByProfileAsync(long profileId, int page, int pageSize, CancellationToken ct = default);
    Task<DiaryDetailDto?> UpdateAsync(long id, UpdateDiaryRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
}
