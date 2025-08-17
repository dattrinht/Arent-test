namespace HealthApp.Application.Features.Meals.Specifications;

public sealed record MealSummaryDto(
    long Id,
    long ProfileId,
    string? Name,
    EnumMealType Type,
    string? Image,
    DateTime DoneAt
);

public sealed class MealsByProfileIdPagingSpec
    : SimplePagingSpecification<Meal, MealSummaryDto>
{
    public MealsByProfileIdPagingSpec(long profileId, int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        Criteria = m => m.ProfileId == profileId;
        Selector = m => new MealSummaryDto(
            m.Id,
            m.ProfileId,
            m.Name,
            m.Type,
            m.Image,
            m.DoneAt
        );
        OrderBy = m => m.Id;
    }
}