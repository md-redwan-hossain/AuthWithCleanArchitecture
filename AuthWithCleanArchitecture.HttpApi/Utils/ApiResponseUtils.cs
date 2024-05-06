using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace AuthWithCleanArchitecture.HttpApi.Utils;

public static class ApiResponseUtils
{
    public static ApiResponse TimeoutResponse()
    {
        return new ApiResponse
        {
            Success = false,
            Code = StatusCodes.Status408RequestTimeout,
            Message = ReasonPhrases.GetReasonPhrase(StatusCodes.Status408RequestTimeout)
        };
    }

    public static string TimeoutResponseJsonString()
    {
        return JsonSerializer.Serialize(TimeoutResponse());
    }

    public static ApiResponse InternalServerErrorResponse()
    {
        return new ApiResponse
        {
            Success = false,
            Code = StatusCodes.Status500InternalServerError,
            Message = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError)
        };
    }


    public static ApiResponse ResponseMaker(int code, object? response = null, string? message = null)
    {
        var isSuccess = code is >= 200 and < 300;
        var res = new ApiResponse
        {
            Success = isSuccess,
            Message = message ?? ReasonPhrases.GetReasonPhrase(code),
            Code = code,
            Data = response
        };

        return res;
    }
}