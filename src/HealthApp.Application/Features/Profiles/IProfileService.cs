namespace HealthApp.Application.Features.Profiles;

public interface IProfileService
{
    public Task<CreateProfileResponse> CreateProfileAsync(CreateProfileRequest req, CancellationToken ct = default);
    Task<(IReadOnlyList<ProfileSummaryDto> Items, int TotalCount)> FetchProfilesByUserAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken ct = default
    );
}
