namespace HealthApp.Domain.Models.ColumnModels;

public interface IColumnRepository : IRepository<Column>
{
    Task<Column> CreateAsync(ColumnDetailDto dto, CancellationToken ct = default);
    Task<Column?> UpdateAsync(ColumnDetailDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<ColumnDetailDto?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<ColumnDetailDto?> FindBySlugAsync(string slug, CancellationToken ct = default);
    public Task<(IReadOnlyList<ColumnSummaryDto> Items, int TotalCount)> FetchAsync(
        long? categoryId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default
    );
}