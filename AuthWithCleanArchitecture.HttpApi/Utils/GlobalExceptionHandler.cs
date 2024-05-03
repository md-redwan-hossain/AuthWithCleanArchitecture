using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;

namespace AuthWithCleanArchitecture.HttpApi.Utils;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );


        if (exception is BadHttpRequestException httpRequestException)
        {
            var errMessage = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest);

            if (httpRequestException.Message.Contains("Required parameter"))
            {
                const string pattern = "\\s*\"[^\"]*\"\\s*";
                const string replacement = " ";
                errMessage = Regex.Replace(httpRequestException.Message, pattern, replacement);
            }

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                ApiResponseUtils.ResponseMaker(StatusCodes.Status400BadRequest, message: errMessage),
                typeof(ApiResponse),
                cancellationToken
            );


            return true;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            ApiResponseUtils.InternalServerErrorResponse(),
            typeof(ApiResponse),
            cancellationToken
        );

        return true;
    }
}