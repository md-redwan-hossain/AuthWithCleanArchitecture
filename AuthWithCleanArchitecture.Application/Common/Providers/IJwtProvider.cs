using System.Security.Claims;

namespace AuthWithCleanArchitecture.Application.Common.Providers;

public interface IJwtProvider
{
    string GenerateJwt(Dictionary<string, object> claims);
    string GenerateJwt(IEnumerable<Claim> claims);
}