namespace HealthApp.Application.Features.Columns.Specifications;

internal sealed class ColumnTaxonomyByProfileIdPagingSpec
    : SimplePagingSpecification<ColumnTaxonomy, ColumnTaxonomySummaryDto>
{
    public ColumnTaxonomyByProfileIdPagingSpec(long profileId, int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        ApplyCriteria(e => e.ProfileId == profileId);
        OrderBy = e => e.Id;
        Selector = e => new ColumnTaxonomySummaryDto(
            e.Id,
            e.Name,
            e.Type
        );
    }
}
