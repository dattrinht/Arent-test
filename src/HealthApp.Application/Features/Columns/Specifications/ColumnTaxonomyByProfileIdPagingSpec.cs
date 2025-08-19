namespace HealthApp.Application.Features.Columns.Specifications;

internal sealed class ColumnTaxonomyByProfileIdPagingSpec
    : SimplePagingSpecification<ColumnTaxonomy, ColumnTaxonomySummaryDto>
{
    public ColumnTaxonomyByProfileIdPagingSpec(int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        OrderBy = e => e.Id;
        Selector = e => new ColumnTaxonomySummaryDto(
            e.Id,
            e.Name,
            e.Type
        );
    }
}
