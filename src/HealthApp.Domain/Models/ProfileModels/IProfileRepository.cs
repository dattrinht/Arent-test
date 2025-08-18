namespace HealthApp.Domain.Models.ProfileModels;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile> SaveAsync(Profile user, CancellationToken ct = default);
    Task<Profile?> FindByIdAsync(long id, CancellationToken ct = default);
    Task<(IReadOnlyList<T> Items, int TotalCount)> FetchByUserIdAsync<T>(
        ISimplePagingSpecification<Profile, T> spec,
        CancellationToken ct = default
    );
}
