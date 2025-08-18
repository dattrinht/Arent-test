namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateExerciseResponse>> Create([FromBody] CreateExerciseRequest body, CancellationToken ct)
    {
        var result = await _exerciseService.CreateAsync(body, ct);
        return result;
    }

    [HttpGet("fetch")]
    public async Task<ActionResult<PagingResult<ExerciseSummaryDto>>> FetchByProfile(
        [FromQuery] long profileId,
        [FromQuery] string? byDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        DateOnly byDateOnly = default;
        var hasByDate = !string.IsNullOrWhiteSpace(byDate);
        if (hasByDate && !Utils.TryParseYearMonth(byDate!, out byDateOnly))
        {
            return BadRequest("Invalid byDate. Expected format yyyy-MM.");
        }

        var (items, total) = await _exerciseService.FetchByProfileAsync(profileId, hasByDate ? byDateOnly : null, page, pageSize, ct);
        return PagingResult<ExerciseSummaryDto>.Create(items, total, page, pageSize);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ExerciseSummaryDto>> Update([FromRoute] long id, [FromBody] UpdateExerciseRequest body, CancellationToken ct)
    {
        var result = await _exerciseService.UpdateAsync(id, body, ct);
        return result is null ? NotFound() : result;
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] long id,
        CancellationToken ct
    )
    {
        var result = await _exerciseService.DeleteAsync(id, ct);
        return result;
    }

    [HttpGet("/achievement")]
    public async Task<ActionResult<ExerciseAchievementDto>> GetAchievementByDate(
        [FromQuery] long profileId,
        [FromQuery] DateOnly day,
        CancellationToken ct = default
    )
    {
        var result = await _exerciseService.GetAchievementByDateAsync(profileId, day, ct);
        return result;
    }
}
