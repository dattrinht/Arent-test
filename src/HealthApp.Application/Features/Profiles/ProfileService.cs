namespace HealthApp.Application.Features.Profiles;

internal class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<CreateProfileResponse> CreateProfileAsync(CreateProfileRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var firstName = req.FisrtName?.Trim();
        var lastName = req.LastName?.Trim();

        ArgumentException.ThrowIfNullOrEmpty(firstName);
        ArgumentException.ThrowIfNullOrEmpty(lastName);

        var now = DateTime.UtcNow;
        var profile = new Profile
        {
            Id = IdGenHelper.CreateId(),
            UserId = req.UserId,
            FirstName = firstName,
            LastName = lastName,
            Birthday = new DateOnly(2000, 01, 01),
            Sex = req.Sex,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        profile = await _profileRepository.SaveAsync(profile, ct);

        return new CreateProfileResponse(profile.UserId, profile.Id);
    }

    public async Task<(IReadOnlyList<ProfileSummaryDto> Items, long TotalCount)> FetchProfilesByUserAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var spec = new ProfilesByUserIdPagingSpec(userId, page, pageSize);
        return await _profileRepository.FetchByUserIdAsync(spec, ct);
    }
}
