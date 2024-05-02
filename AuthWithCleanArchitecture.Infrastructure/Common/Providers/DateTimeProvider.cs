using AuthWithCleanArchitecture.Application.Common.Providers;

namespace AuthWithCleanArchitecture.Infrastructure.Common.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentUtcTime => TimeProvider.System.GetUtcNow().UtcDateTime;
    public DateTime CurrentLocalTime => TimeProvider.System.GetLocalNow().LocalDateTime;
    public string Iso8601RoundTripString(DateTime dateTime) => dateTime.ToString("o");
}