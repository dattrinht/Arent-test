
namespace HealthApp.Domain.Models.BodyRecordModels;

public interface IBodyRecordRepository : IRepository<BodyRecord>
{
    Task<BodyRecord> SaveAsync(BodyRecord bodyRecord, CancellationToken ct = default);
    Task<BodyRecord?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<BodyRecordMonthlyAggregateDto>> FetchMonthlyAverageAsync(
        long profileId,
        DateOnly fromMonth,
        DateOnly toMonth,
        CancellationToken ct = default);
}
