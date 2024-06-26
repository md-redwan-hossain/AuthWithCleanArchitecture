using System.ComponentModel.DataAnnotations;

namespace AuthWithCleanArchitecture.Application.Common.Options;

public record JwtOptions
{
    public const string SectionName = "JwtOptions";
    [Required] public required string Secret { get; init; }
    [Required] public required uint ExpiryMinutes { get; init; }
}