namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class MealsController : ControllerBase
{
    private readonly IMealService _mealService;
    public MealsController(IMealService mealService)
    {
        _mealService = mealService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateMealResponse>> Create([FromBody] CreateMealRequest body, CancellationToken ct)
    {
        var result = await _mealService.CreateAsync(body, ct);
        return result;
    }

    [HttpGet("{profileId:long}")]
    public async Task<ActionResult<PagingResult<MealSummaryDto>>> FetchByProfile(
        [FromRoute] long profileId,
        [FromQuery] EnumMealType? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var result = await _mealService.FetchByProfileAsync(profileId, type, page, pageSize, ct);
        return PagingResult<MealSummaryDto>.Create(result.Items, result.TotalCount, page, pageSize);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] long id, CancellationToken ct)
    {
        var result = await _mealService.DeleteAsync(id, ct);
        return result;
    }
}
