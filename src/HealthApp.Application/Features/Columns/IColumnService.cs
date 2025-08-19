namespace HealthApp.Application.Features.Columns;

public interface IColumnService
{
    Task<CreateColumnResponse> CreateAsync(CreateColumnRequest req, CancellationToken ct = default);
    Task<ColumnDetailDto?> UpdateAsync(long id, UpdateColumnRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<ColumnSummaryDto> Items, int TotalCount)> FetchAsync(
        long? categoryId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);
    Task<ColumnDetailDto?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<ColumnDetailDto?> FindBySlugAsync(string slug, CancellationToken ct = default);
}
