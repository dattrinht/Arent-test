namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;
    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("fetch")]
    public async Task<ActionResult<PagingResult<ProfileSummaryDto>>> FetchByUser(
        [FromQuery] long userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var result = await _profileService.FetchProfilesByUserAsync(userId, page, pageSize, ct);
        return PagingResult<ProfileSummaryDto>.Create(result.Items, result.TotalCount, page, pageSize);
    }
}
