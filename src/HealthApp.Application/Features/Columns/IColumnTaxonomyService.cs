namespace HealthApp.Application.Features.Columns;

public interface IColumnTaxonomyService
{
    Task<CreateColumnTaxonomyResponse> CreateAsync(CreateColumnTaxonomyRequest req, CancellationToken ct = default);
    Task<(IReadOnlyList<ColumnTaxonomySummaryDto> Items, long TotalCount)> FetchByProfileAsync(long profileId, int page, int pageSize, CancellationToken ct = default);
    Task<ColumnTaxonomySummaryDto?> UpdateAsync(long id, UpdateColumnTaxonomyRequest req, CancellationToken ct = default);
}
