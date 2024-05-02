using System.Security.Claims;
using System.Text;
using AuthWithCleanArchitecture.Application.Common.Options;
using AuthWithCleanArchitecture.Application.Common.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthWithCleanArchitecture.Infrastructure.Common.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateJwt(Dictionary<string, object> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            IssuedAt = _dateTimeProvider.CurrentUtcTime,
            Expires = _dateTimeProvider.CurrentUtcTime.AddMinutes(_jwtOptions.ExpiryMinutes),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler { SetDefaultTimesOnTokenCreation = false };
        return handler.CreateToken(descriptor);
    }

    public string GenerateJwt(IEnumerable<Claim> claims)
    {
        Dictionary<string, object> storage = [];

        foreach (var claim in claims)
        {
            storage.Add(claim.Type, claim.Value);
        }

        return GenerateJwt(storage);
    }
}