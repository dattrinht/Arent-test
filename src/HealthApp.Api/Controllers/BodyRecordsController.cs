using System.Globalization;

namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class BodyRecordsController : ControllerBase
{
    private readonly IBodyRecordService _bodyRecordSerivce;
    public BodyRecordsController(IBodyRecordService bodyRecordSerivce)
    {
        _bodyRecordSerivce = bodyRecordSerivce;
    }

    [HttpPost]
    public async Task<ActionResult<CreateBodyRecordResponse>> Create([FromBody] CreateBodyRecordRequest body, CancellationToken ct)
    {
        var result = await _bodyRecordSerivce.CreateAsync(body, ct);
        return result;
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] long id, CancellationToken ct)
    {
        var result = await _bodyRecordSerivce.DeleteAsync(id, ct);
        return result;
    }

    [HttpGet("{profileId:long}/monthly-averages")]
    public async Task<ActionResult<IReadOnlyList<BodyRecordMonthlyAggregateDto>>> FetchMonthlyAverages(
        [FromRoute] long profileId,
        [FromQuery] string? fromMonth,
        [FromQuery] string? toMonth,
        CancellationToken ct = default
    )
    {
        DateOnly toMonthDateOnly;
        if (string.IsNullOrWhiteSpace(toMonth))
        {
            var now = DateTime.UtcNow;
            toMonthDateOnly = new DateOnly(now.Year, now.Month, 1);
        }
        else if (!TryParseYearMonth(toMonth!, out toMonthDateOnly))
        {
            return BadRequest("Invalid toMonth. Expected format yyyy-MM.");
        }

        DateOnly fromMonthDateOnly;
        if (string.IsNullOrWhiteSpace(fromMonth))
        {
            var anchorDate = DateTime.UtcNow.AddMonths(-12);
            fromMonthDateOnly = new DateOnly(anchorDate.Year, anchorDate.Month, 1);
        }
        else if (!TryParseYearMonth(fromMonth!, out fromMonthDateOnly))
        {
            return BadRequest("Invalid fromMonth. Expected format yyyy-MM.");
        }

        if (fromMonthDateOnly > toMonthDateOnly)
        {
            return BadRequest("fromMonth must be earlier than or equal to toMonth.");
        }

        var result = await _bodyRecordSerivce.FetchMonthlyAveragesAsync(profileId, fromMonthDateOnly, toMonthDateOnly, ct);
        return Ok(result);
    }

    private static bool TryParseYearMonth(string ym, out DateOnly month)
    {
        return DateOnly.TryParseExact(
            ym + "-01",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out month
        );
    }
}
