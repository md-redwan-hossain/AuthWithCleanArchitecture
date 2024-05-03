using System.Security.Claims;
using AuthWithCleanArchitecture.Application.MembershipFeatures;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;
using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
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


        accountRoutes.MapGet("/profile", Profile)
            .RequireAuthorization()
            .Produces<ApiResponse<string>>(statusCode: StatusCodes.Status200OK)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status401Unauthorized)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status403Forbidden)
            .Produces<ApiResponse>(statusCode: StatusCodes.Status404NotFound);
    }


    private static async Task<IResult> SignUp([FromBody] AppUserSignUpRequest dto,
        [FromServices] IMembershipService memberService)
    {
        var result = await memberService.SignUpAsync(dto);

        return await result.MatchAsync(OnGoodOutcome, OnBadOutcome);

        async Task<IResult> OnGoodOutcome(AppUser res)
        {
            return await ApiEndpointResponse.SendAsync<AppUser, AppUserSignUpResponse>
            (StatusCodes.Status201Created, res);
        }

        IResult OnBadOutcome(SignUpBadOutcome err)
        {
            return ApiEndpointResponse.Send(err switch
            {
                SignUpBadOutcome.Duplicate => BadOutcomeTag.Duplicate,
                _ => BadOutcomeTag.Unknown
            });
        }
    }

    private static async Task<IResult> Login([FromBody] AppUserLoginRequest dto,
        [FromServices] IMembershipService memberService)
    {
        var result = await memberService.LoginAsync(dto);

        return result.Match(OnGoodOutcome, OnBadOutcome);

        IResult OnGoodOutcome(string jwt)
        {
            return ApiEndpointResponse.Send(StatusCodes.Status200OK, data: jwt);
        }

        IResult OnBadOutcome(LoginBadOutcome err)
        {
            return ApiEndpointResponse.Send(err switch
            {
                LoginBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                LoginBadOutcome.PasswordNotMatched => BadOutcomeTag.Unauthorized,
                _ => BadOutcomeTag.Unknown
            });
        }
    }


    private static async Task<IResult> Profile(ClaimsPrincipal user,
        [FromServices] IMembershipService memberService)
    {
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return ApiEndpointResponse.Send(StatusCodes.Status401Unauthorized);
        }

        var result = await memberService.ProfileAsync(new AppUserId(Guid.Parse(currentUserId)));
        return await result.MatchAsync(OnGoodOutcome, OnBadOutcome);

        async Task<IResult> OnGoodOutcome(AppUser res)
        {
            return await ApiEndpointResponse.SendAsync<AppUser, AppUserProfileResponse>
                (StatusCodes.Status200OK, res);
        }

        IResult OnBadOutcome(ProfileBadOutcome err)
        {
            return ApiEndpointResponse.Send(err switch
            {
                ProfileBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                ProfileBadOutcome.Banned => BadOutcomeTag.Forbidden,
                ProfileBadOutcome.LockedOut => BadOutcomeTag.Forbidden,
                _ => BadOutcomeTag.Unknown
            });
        }
    }
}