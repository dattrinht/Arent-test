namespace HealthApp.Application.Features.Meals.Specifications;

public sealed class MealsByProfileIdPagingSpec
    : SimplePagingSpecification<Meal, MealSummaryDto>
{
    public MealsByProfileIdPagingSpec(
        long profileId,
        EnumMealType? type,
        int page, 
        int pageSize
    )
    {
        ApplyPaging(page, pageSize);
        ApplyCriteria(m => m.ProfileId == profileId);
        if (type.HasValue)
        {
            ApplyCriteria(m => m.Type == type.Value);
        }
        Selector = m => new MealSummaryDto(
            m.Id,
            m.ProfileId,
            m.Name,
            m.Type,
            m.Image,
            m.DoneAt
        );
        OrderBy = m => m.Id;
        OrderByDescending = m => m.DoneAt;
    }
}