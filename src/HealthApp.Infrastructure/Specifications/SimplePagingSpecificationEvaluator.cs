namespace HealthApp.Infrastructure.Specifications;

internal static class SimplePagingSpecificationEvaluator
{
    public static IQueryable<TResult> GetQuery<T, TResult>(
        IQueryable<T> inputQuery,
        ISimplePagingSpecification<T, TResult> spec,
        out IQueryable<T> filtered
    )  where T : class
    {
        var query = inputQuery;

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        filtered = query;

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);
        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPagingEnabled)
        {
            if (spec.Skip is > 0) query = query.Skip(spec.Skip.Value);
            if (spec.Take is > 0) query = query.Take(spec.Take.Value);
        }

        query = query.AsNoTracking();

        return query.Select(spec.Selector!);
    }
}
