namespace HealthApp.Domain.Models.ColumnModels;

public interface IColumnTaxonomyRepository : IRepository<ColumnTaxonomy>
{
    Task<ColumnTaxonomy> SaveAsync(ColumnTaxonomy taxonomy, CancellationToken ct = default);
    Task<ColumnTaxonomy?> UpdateAsync(ColumnTaxonomySummaryDto dto, CancellationToken ct = default);
    Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<ColumnTaxonomy, TResult> spec,
        CancellationToken ct = default
    );
}
