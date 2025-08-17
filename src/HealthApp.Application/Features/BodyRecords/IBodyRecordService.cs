namespace HealthApp.Application.Features.BodyRecords;

public interface IBodyRecordService
{
    Task<CreateBodyRecordResponse> CreateAsync(CreateBodyRecordRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<BodyRecordSummaryDto?> UpdateAsync(long id, UpdateBodyRecordRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>> FetchMonthlyAveragesAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        CancellationToken ct = default
    );
}
