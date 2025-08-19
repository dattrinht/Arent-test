namespace HealthApp.Application.Features.Exercises.Specifications;

internal sealed class ExercisesByProfileIdPagingSpec
    : SimplePagingSpecification<Exercise, ExerciseSummaryDto>
{
    public ExercisesByProfileIdPagingSpec(long profileId, DateOnly? byDate, int page, int pageSize)
    {
        ApplyPaging(page, pageSize);
        ApplyCriteria(e => e.ProfileId == profileId);
        if (byDate is not null)
        {
            var start = new DateTime(byDate.Value.Year, byDate.Value.Month, byDate.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddDays(1);

            ApplyCriteria(e => e.FinishedAt >= start && e.FinishedAt < end);
        }
        OrderBy = e => e.Id;

        Selector = e => new ExerciseSummaryDto(
            e.Id,
            e.ProfileId,
            e.Name,
            e.Type,
            e.Status,
            e.DurationSec,
            e.CaloriesBurned,
            e.FinishedAt
        );
    }
}
