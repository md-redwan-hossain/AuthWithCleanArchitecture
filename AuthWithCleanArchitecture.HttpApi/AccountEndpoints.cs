using AuthWithCleanArchitecture.Application.MembershipFeatures;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Outcomes;
using AuthWithCleanArchitecture.HttpApi.Utils;
using Microsoft.AspNetCore.Mvc;
using SharpOutcome;

namespace AuthWithCleanArchitecture.HttpApi;

public class AccountEndpoints : IApiEndpoint
{
    public static void DefineRoutes(IEndpointRouteBuilder routes)
    {
        var accountRoutes = routes.MapGroup("api/account/");

        accountRoutes.MapPost("/signup", SignUp)
            .AllowAnonymous()
            .AddEndpointFilter<FluentValidationFilter<AppUserSignUpRequest>>()
            .Produces<ApiResponse<AppUserSignUpResponse>>(statusCode: StatusCodes.Status201Created)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status409Conflict);


        accountRoutes.MapPost("/login", Login)
            .AllowAnonymous()
            .AddEndpointFilter<FluentValidationFilter<AppUserLoginRequest>>()
            .Produces<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status404NotFound);
    }


    private static async Task<IResult> SignUp([FromBody] AppUserSignUpRequest dto,
        [FromServices] IMembershipService memberService)
    {
        var result = await memberService.SignUpAsync(dto);

        return result.Match(
            res => ApiEndpointResponse.Send(StatusCodes.Status201Created, res),
            err => ApiEndpointResponse.Send(err switch
            {
                SignUpBadOutcome.Duplicate => BadOutcomeTag.Duplicate,
                _ => BadOutcomeTag.Unknown
            })
        );
    }

    private static async Task<IResult> Login([FromBody] AppUserLoginRequest dto,
        [FromServices] IMembershipService memberService)
    {
        var result = await memberService.LoginAsync(dto);

        return result.Match(
            res => ApiEndpointResponse.Send(StatusCodes.Status200OK, data: res),
            err => ApiEndpointResponse.Send(err switch
            {
                LoginBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                LoginBadOutcome.PasswordNotMatched => BadOutcomeTag.Unauthorized,
                _ => BadOutcomeTag.Unknown
            })
        );
    }
}