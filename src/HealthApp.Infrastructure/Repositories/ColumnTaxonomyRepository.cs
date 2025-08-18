namespace HealthApp.Infrastructure.Repositories;

internal class ColumnTaxonomyRepository : IColumnTaxonomyRepository
{
    private readonly HealthAppContext _dbContext;

    public ColumnTaxonomyRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ColumnTaxonomy> SaveAsync(ColumnTaxonomy taxonomy, CancellationToken ct = default)
    {
        _dbContext.ColumnTaxonomies.Add(taxonomy);
        await _dbContext.SaveChangesAsync(ct);
        return taxonomy;
    }

    public async Task<ColumnTaxonomy?> UpdateAsync(ColumnTaxonomySummaryDto dto, CancellationToken ct = default)
    {
        var entity = await _dbContext.ColumnTaxonomies
                .FirstOrDefaultAsync(t => t.Id == dto.Id, ct);

        if (entity is null) return null;

        entity.Name = dto.Name?.Trim() ?? entity.Name;

        await _dbContext.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<(IReadOnlyList<TResult> Items, long TotalCount)> FetchByProfileIdAsync<TResult>(
        ISimplePagingSpecification<ColumnTaxonomy, TResult> spec,
        CancellationToken ct = default
    )
    {
        IQueryable<ColumnTaxonomy> baseQuery = _dbContext.ColumnTaxonomies;

        var projected = SimplePagingSpecificationEvaluator.GetQuery(
            baseQuery, spec, out var filteredBeforePaging);

        var total = await filteredBeforePaging.LongCountAsync(ct);
        var items = await projected.ToListAsync(ct);

        return (items, total);
    }
}
