namespace HealthApp.Application.Features.Diaries.Specifications;

public sealed class DiariesByProfileIdPagingSpec
    : SimplePagingSpecification<Diary, DiarySummaryDto>
{
    public DiariesByProfileIdPagingSpec(long profileId, int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        ApplyCriteria(d => d.ProfileId == profileId);
        OrderByDescending = d => d.WrittenAt;
        OrderBy = d => d.Id;

        Selector = d => new DiarySummaryDto(
            d.Id,
            d.ProfileId,
            d.Title,
            d.Preview,
            d.WrittenAt
        );
    }
}