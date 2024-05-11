using System.Security.Claims;
using AuthWithCleanArchitecture.Application.MembershipFeatures;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;
using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using AuthWithCleanArchitecture.HttpApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpOutcome.Helpers;

namespace AuthWithCleanArchitecture.HttpApi.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    [HttpPost("signup")]
    [AllowAnonymous]
    [ValidationActionFilter<AppUserSignUpRequest>]
    [ProducesResponseType<ApiResponse<AppUserSignUpResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp([FromBody] AppUserSignUpRequest dto, IMembershipService memberService)
    {
        var result = await memberService.SignUpAsync(dto);

        return await result.MatchAsync(OnGoodOutcome, OnBadOutcome);

        async Task<IActionResult> OnGoodOutcome(AppUser res)
        {
            return await ControllerContext.MakeResponseAsync<AppUser, AppUserSignUpResponse>
                (StatusCodes.Status201Created, res);
        }

        IActionResult OnBadOutcome(SignUpBadOutcome err)
        {
            return ControllerContext.MakeResponse(err switch
            {
                SignUpBadOutcome.Duplicate => BadOutcomeTag.Duplicate,
                _ => BadOutcomeTag.Unknown
            });
        }
    }


    [HttpPost("login")]
    [AllowAnonymous]
    [ValidationActionFilter<AppUserLoginRequest>]
    public async Task<IActionResult> Login([FromBody] AppUserLoginRequest dto, IMembershipService memberService)
    {
        var result = await memberService.LoginAsync(dto);

        return result.Match(OnGoodOutcome, OnBadOutcome);

        IActionResult OnGoodOutcome(string jsonWebToken)
        {
            return ControllerContext.MakeResponse(StatusCodes.Status200OK, data: jsonWebToken);
        }

        IActionResult OnBadOutcome(LoginBadOutcome err)
        {
            return ControllerContext.MakeResponse(err switch
            {
                LoginBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                LoginBadOutcome.PasswordNotMatched => BadOutcomeTag.Unauthorized,
                _ => BadOutcomeTag.Unknown
            });
        }
    }

    [HttpGet("profile")]
    public async Task<IActionResult> ReadProfile(IMembershipService memberService)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return ControllerContext.MakeResponse(StatusCodes.Status401Unauthorized);
        }

        var result = await memberService.ReadProfileAsync(new AppUserId { Data = Guid.Parse(currentUserId) });
        return await result.MatchAsync(OnGoodOutcome, OnBadOutcome);

        async Task<IActionResult> OnGoodOutcome(AppUser res)
        {
            return await ControllerContext.MakeResponseAsync<AppUser, AppUserProfileResponse>
                (StatusCodes.Status200OK, res);
        }

        IActionResult OnBadOutcome(ProfileBadOutcome err)
        {
            return ControllerContext.MakeResponse(err switch
            {
                ProfileBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                ProfileBadOutcome.Banned => BadOutcomeTag.Forbidden,
                ProfileBadOutcome.LockedOut => BadOutcomeTag.Forbidden,
                _ => BadOutcomeTag.Unknown
            });
        }
    }


    [HttpPut("profile")]
    [Authorize(Policy = "DataUpdatePolicy")]
    [ValidationActionFilter<AppUserProfileUpdateRequest>]
    public async Task<IActionResult> UpdateProfile([FromBody] AppUserProfileUpdateRequest dto,
        IMembershipService memberService)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
        {
            return ControllerContext.MakeResponse(StatusCodes.Status401Unauthorized);
        }

        var result = await memberService.UpdateProfileAsync(new AppUserId { Data = Guid.Parse(currentUserId) }, dto);
        return await result.MatchAsync(OnGoodOutcome, OnBadOutcome);

        async Task<IActionResult> OnGoodOutcome(AppUser res)
        {
            return await ControllerContext.MakeResponseAsync<AppUser, AppUserProfileResponse>
                (StatusCodes.Status200OK, res);
        }

        IActionResult OnBadOutcome(ProfileBadOutcome err)
        {
            return ControllerContext.MakeResponse(err switch
            {
                ProfileBadOutcome.UserNotFound => BadOutcomeTag.NotFound,
                ProfileBadOutcome.Banned => BadOutcomeTag.Forbidden,
                ProfileBadOutcome.LockedOut => BadOutcomeTag.Forbidden,
                _ => BadOutcomeTag.Unknown
            });
        }
    }
}