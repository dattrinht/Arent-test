namespace HealthApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService auth)
    {
        _authService = auth;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest req)
    {
        return Ok(await _authService.RegisterAsync(req));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest req)
    {
        return Ok(await _authService.LoginAsync(req));
    }

    [HttpGet("secret")]
    public ActionResult<string> Secret()
    {
        return Ok("SUPER_SECRET_DEMO_ONLY");
    }
}
