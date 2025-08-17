namespace HealthApp.Application.Features.Profiles;

internal class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(IProfileRepository profileRepository, ILogger<ProfileService> logger)
    {
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateProfileResponse> CreateProfileAsync(CreateProfileRequest req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);

        var firstName = req.FisrtName?.Trim();
        var lastName = req.LastName?.Trim();

        ArgumentException.ThrowIfNullOrEmpty(firstName);
        ArgumentException.ThrowIfNullOrEmpty(lastName);

        var now = DateTime.UtcNow;
        var entity = new Profile
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

        entity = await _profileRepository.SaveAsync(entity, ct);
        _logger.LogInformation("Profile created: {ProfileId} for entity {UserId}", entity.Id, entity.UserId);

        return new CreateProfileResponse(entity.UserId, entity.Id);
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
