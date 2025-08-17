namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class IdentitiesController : ControllerBase
{
    private readonly IAuthService _authService;
    public IdentitiesController(IAuthService auth)
    {
        _authService = auth;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest req, CancellationToken ct)
    {
        return await _authService.RegisterAsync(req, ct);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest req, CancellationToken ct)
    {
        return await _authService.LoginAsync(req, ct);
    }

    [HttpGet("secret")]
    public ActionResult<string> Secret()
    {
        return "SUPER_SECRET_DEMO_ONLY";
    }
}
