using System.ComponentModel.DataAnnotations;

namespace AuthWithCleanArchitecture.Application.Common.Options;

public record AppSecretOptions
{
    public const string SectionName = "AppSecretOptions";
    [Required] public required string DatabaseUrl { get; init; }
}