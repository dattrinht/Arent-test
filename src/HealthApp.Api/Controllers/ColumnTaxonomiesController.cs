namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class ColumnTaxonomiesController : ControllerBase
{
    private readonly IColumnTaxonomyService _columnTaxonomyService;

    public ColumnTaxonomiesController(IColumnTaxonomyService columnTaxonomyService)
    {
        _columnTaxonomyService = columnTaxonomyService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateColumnTaxonomyResponse>> Create([FromBody] CreateColumnTaxonomyRequest body, CancellationToken ct)
    {
        var result = await _columnTaxonomyService.CreateAsync(body, ct);
        return result;
    }


    [HttpGet("fetch")]
    public async Task<ActionResult<PagingResult<ColumnTaxonomySummaryDto>>> FetchByProfile(
        [FromQuery] long profileId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (items, total) = await _columnTaxonomyService.FetchByProfileAsync(profileId, page, pageSize, ct);
        return PagingResult<ColumnTaxonomySummaryDto>.Create(items, total, page, pageSize);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ColumnTaxonomySummaryDto>> Update([FromRoute] long id, [FromBody] UpdateColumnTaxonomyRequest body, CancellationToken ct)
    {
        var result = await _columnTaxonomyService.UpdateAsync(id, body, ct);
        return result is null ? NotFound() : result;
    }
}
