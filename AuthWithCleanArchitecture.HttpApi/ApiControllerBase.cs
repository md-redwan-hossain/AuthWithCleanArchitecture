using System.Net;
using AuthWithCleanArchitecture.HttpApi.Utils;
using FluentValidation.Results;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SharpOutcome;

namespace AuthWithCleanArchitecture.HttpApi;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult SendResponse(HttpStatusCode code, object data, string message)
    {
        return ResponseMaker(code, data, message);
    }

    protected IActionResult SendResponse(HttpStatusCode code, object data)
    {
        return ResponseMaker(code, data, null);
    }

    protected IActionResult SendResponse(HttpStatusCode code, string message)
    {
        return ResponseMaker(code, null, message);
    }


    protected IActionResult SendResponse(HttpStatusCode code)
    {
        return ResponseMaker(code, null, null);
    }

    protected IActionResult SendResponse(HttpStatusCode code, IEnumerable<ValidationFailure> data)
    {
        var errors = data.Select(error =>
        {
            var errorInfo = new Dictionary<string, object>
            {
                { "propertyName", error.FormattedMessagePlaceholderValues["PropertyName"] },
                { "errorMessage", error.ErrorMessage },
                { "attemptedValue", error.AttemptedValue }
            };

            if (error.FormattedMessagePlaceholderValues.TryGetValue("CollectionIndex", out var index))
            {
                errorInfo["collectionIndex"] = index;
            }

            return errorInfo;
        });

        return ResponseMaker(code, errors, null);
    }


    protected IActionResult SendResponse(IEnumerable<ValidationFailure> data)
    {
        return SendResponse(HttpStatusCode.BadRequest, data);
    }


    protected async Task<IActionResult> SendResponseAsync<TEntity, TResponse>(HttpStatusCode code, TEntity data)
    {
        return ResponseMaker(code, await data.BuildAdapter().AdaptToTypeAsync<TResponse>(), null);
    }

    protected async Task<IActionResult> SendResponseAsync<TEntity, TResponse>(HttpStatusCode code,
        IEnumerable<TEntity> data)
    {
        return ResponseMaker(
            code,
            await Task.WhenAll(data.Select(x => x.BuildAdapter().AdaptToTypeAsync<TResponse>())),
            null
        );
    }


    protected IActionResult SendResponse(IBadOutcome badOutcome)
    {
        var code = badOutcome.Tag switch
        {
            BadOutcomeTag.BadRequest => HttpStatusCode.BadRequest,
            BadOutcomeTag.Unprocessable => HttpStatusCode.UnprocessableEntity,
            BadOutcomeTag.Conflict => HttpStatusCode.Conflict,
            BadOutcomeTag.Duplicate => HttpStatusCode.Conflict,
            BadOutcomeTag.NotFound => HttpStatusCode.NotFound,
            BadOutcomeTag.Unauthorized => HttpStatusCode.Unauthorized,
            BadOutcomeTag.Forbidden => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError,
        };

        return ResponseMaker(code, null, badOutcome.Reason);
    }


    protected IActionResult SendResponse(IGoodOutcome goodOutcome)
    {
        var code = goodOutcome.Tag switch
        {
            GoodOutcomeTag.Ok => HttpStatusCode.OK,
            GoodOutcomeTag.Created => HttpStatusCode.Created,
            GoodOutcomeTag.Deleted => HttpStatusCode.NoContent,
            _ => HttpStatusCode.OK,
        };

        return ResponseMaker(code, null, goodOutcome.Reason);
    }

    private IActionResult ResponseMaker(HttpStatusCode code, object? data, string? message)
    {
        if (code == HttpStatusCode.NoContent) return NoContent();


        var castedCode = (int)code;
        var isSuccess = castedCode is >= 200 and < 300;
        var res = new ApiResponse
        {
            Success = isSuccess,
            Message = message ?? ReasonPhrases.GetReasonPhrase(castedCode),
            Code = castedCode,
            Data = data
        };

        return StatusCode(castedCode, res);
    }
}