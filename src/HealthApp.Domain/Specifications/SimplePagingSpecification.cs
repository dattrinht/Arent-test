using System.Linq.Expressions;

namespace HealthApp.Domain.Specifications;

public interface ISimplePagingSpecification<T, TResult>
{
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    Expression<Func<T, TResult>>? Selector { get; }
    int? Skip { get; }
    int? Take { get; }
    bool IsPagingEnabled { get; }
}

public abstract class SimplePagingSpecification<T, TResult> : ISimplePagingSpecification<T, TResult>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Expression<Func<T, object>>? OrderBy { get; protected set; }
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }
    public Expression<Func<T, TResult>>? Selector { get; protected set; }
    public int? Skip { get; protected set; }
    public int? Take { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }

    protected void ApplyPaging(int? page, int? pageSize)
    {
        var defaultPage = 1;
        var defaultPageSize = 20;
        var maxPageSize = 200;

        var p = (page is null or < 1) ? defaultPage : page.Value;
        var ps = (pageSize is null or < 1) ? defaultPageSize : pageSize.Value;
        if (ps > maxPageSize) ps = maxPageSize;

        Skip = (p - 1) * ps;
        Take = ps;
        IsPagingEnabled = true;
    }

    protected void ApplyCriteria(Expression<Func<T, bool>> newCriteria)
    {
        if (Criteria is null)
        {
            Criteria = newCriteria;
        }
        else
        {
            var param = Expression.Parameter(typeof(T));

            var body = Expression.AndAlso(
                Expression.Invoke(Criteria, param),
                Expression.Invoke(newCriteria, param));

            Criteria = Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
