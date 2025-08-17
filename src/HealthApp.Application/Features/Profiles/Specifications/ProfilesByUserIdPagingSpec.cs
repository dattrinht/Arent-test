namespace HealthApp.Application.Features.Profiles.Specifications;

public sealed class ProfilesByUserIdPagingSpec
    : SimplePagingSpecification<Profile, ProfileSummaryDto>
{
    public ProfilesByUserIdPagingSpec(long userId, int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        Criteria = p => p.UserId == userId;
        Selector = p => new ProfileSummaryDto(p.Id, p.FirstName, p.LastName);
        OrderBy = p => p.Id;
    }
}