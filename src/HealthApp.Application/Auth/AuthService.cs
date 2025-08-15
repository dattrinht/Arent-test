namespace HealthApp.Application.Auth;

public class AuthService : IAuthService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iter = 50_000;

    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> RegisterAsync(AuthRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        if (await _userRepository.FindByEmailAsync(email) is not null)
        {
            throw new InvalidOperationException("Email already registered.");
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(req.Password), salt, Iter, HashAlgorithmName.SHA256, KeySize);

        var user = new User
        {
            Id = IdGenHelper.CreateId(),
            Email = email,
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordHash = Convert.ToBase64String(hash),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user = await _userRepository.CreateAsync(user);

        var token = CreateToken(user);
        return new AuthResponse(user.Id, user.Email, token);
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest req, CancellationToken ct = default)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.FindByEmailAsync(email) ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!Verify(req.Password, user.PasswordSalt, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var token = CreateToken(user);
        return new AuthResponse(user.Id, user.Email, token);
    }

    private static bool Verify(string password, string saltB64, string hashB64)
    {
        var salt = Convert.FromBase64String(saltB64);
        var expected = Convert.FromBase64String(hashB64);
        var actual = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, Iter, HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }

    private static string CreateToken(User user)
    {
        var now = DateTime.UtcNow;
        var exp = now.Add(TimeSpan.FromMinutes(60));

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DEMO_ONLY_SUPER_SECRET_256BIT_KEY_DEMO_ONLY")), SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: "healthapp",
            audience: "healthapp",
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ],
            notBefore: now,
            expires: exp,
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return token;
    }
}
