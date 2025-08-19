namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class ColumnsController : ControllerBase
{
    private readonly IColumnService _columnService;

    public ColumnsController(IColumnService columnService)
    {
        _columnService = columnService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateColumnResponse>> Create([FromBody] CreateColumnRequest body, CancellationToken ct)
    {
        var result = await _columnService.CreateAsync(body, ct);
        return result;
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ColumnDetailDto>> Update([FromRoute] long id, [FromBody] UpdateColumnRequest body, CancellationToken ct)
    {
        var result = await _columnService.UpdateAsync(id, body, ct);
        return result is null ? NotFound() : result;
    }


    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] long id, CancellationToken ct)
    {
        var result = await _columnService.DeleteAsync(id, ct);
        return result;
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<ColumnDetailDto>> GetById([FromRoute] long id, CancellationToken ct)
    {
        var result = await _columnService.FindByIdAsync(id, ct);
        if (result is null) return NotFound();
        return result;
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ColumnDetailDto>> GetBySlug([FromRoute] string slug, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(slug)) return BadRequest("Slug is required.");
        var result = await _columnService.FindBySlugAsync(slug, ct);
        if (result is null) return NotFound();
        return result;
    }

    [HttpGet("fetch")]
    [AllowAnonymous]
    public async Task<ActionResult<PagingResult<ColumnSummaryDto>>> Get(
        [FromQuery] long? categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var (items, total) = await _columnService.FetchAsync(categoryId, page, pageSize, ct);
        return PagingResult<ColumnSummaryDto>.Create(items, total, page, pageSize);
    }
}
