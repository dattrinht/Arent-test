namespace HealthApp.Api.Commons;

public class PagingResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public long TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagingResult() { }

    public PagingResult(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public static PagingResult<T> Create(IReadOnlyList<T> items, long totalCount, int page, int pageSize)
        => new(items, totalCount, page, pageSize);
}
