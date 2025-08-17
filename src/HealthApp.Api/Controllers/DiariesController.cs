namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class DiariesController : ControllerBase
{
    private readonly IDiaryService _diaryService;

    public DiariesController(IDiaryService diaryService)
    {
        _diaryService = diaryService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateDiaryResponse>> Create([FromBody] CreateDiaryRequest body, CancellationToken ct)
    {
        var result = await _diaryService.CreateAsync(body, ct);
        return result;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DiaryDetailDto>> GetById([FromRoute] long id, CancellationToken ct)
    {
        var result = await _diaryService.FindByIdAsync(id, ct);
        return result is null ? NotFound() : result;
    }

    [HttpGet("fetch")]
    public async Task<ActionResult<PagingResult<DiarySummaryDto>>> FetchByProfile(
        [FromQuery] long profileId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var (items, total) = await _diaryService.FetchByProfileAsync(profileId, page, pageSize, ct);
        return PagingResult<DiarySummaryDto>.Create(items, total, page, pageSize);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<DiaryDetailDto>> Update([FromRoute] long id, [FromBody] UpdateDiaryRequest body, CancellationToken ct)
    {
        var result = await _diaryService.UpdateAsync(id, body, ct);
        return result is null ? NotFound() : result;
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] long id,
        CancellationToken ct)
    {
        var result = await _diaryService.DeleteAsync(id, ct);
        return result;
    }
}
