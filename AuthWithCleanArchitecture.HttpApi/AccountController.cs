using System.Net;
using AuthWithCleanArchitecture.Application.MembershipFeatures;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Outcomes;
using AuthWithCleanArchitecture.Domain.AppUserAggregate;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpOutcome;

namespace AuthWithCleanArchitecture.HttpApi;

[Consumes("application/json")]
[Produces("application/json")]
[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    private readonly IValidator<AppUserSignUpRequest> _signUpRequestValidator;
    private readonly IValidator<AppUserLoginRequest> _loginRequestValidator;
    private readonly IMembershipService _membershipService;


    public AccountController(IValidator<AppUserSignUpRequest> signUpRequestValidator,
        IValidator<AppUserLoginRequest> loginRequestValidator, IMembershipService membershipService)
    {
        _signUpRequestValidator = signUpRequestValidator;
        _loginRequestValidator = loginRequestValidator;
        _membershipService = membershipService;
    }

    [AllowAnonymous]
    [HttpPost("join")]
    public async Task<IActionResult> SignUp([FromBody] AppUserSignUpRequest dto)
    {
        var validationResult = await _signUpRequestValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return SendResponse(validationResult.Errors);

        var result = await _membershipService.SignUpAsync(dto);

        return await result.MatchAsync(
            entity => SendResponseAsync<AppUser, AppUserSignUpResponse>(HttpStatusCode.Created, entity),
            err => SendResponse(err switch
            {
                SignUpBadOutcome.Duplicate => new BadOutcome(BadOutcomeTag.Duplicate),
                _ => new BadOutcome(BadOutcomeTag.Unknown)
            })
        );
    }

    //
    // [AllowAnonymous]
    // [HttpPost("login")]
    // public async Task<IActionResult> Login(LoginRequestDto dto)
    // {
    //     var validationResult = await _loginRequestValidator.ValidateAsync(dto);
    //     if (validationResult.IsValid is false) return SendResponse(validationResult.Errors);
    //
    //     var result = await _userService.LoginAsync(dto, HttpContext.RequestAborted);
    //
    //     return result.Match(
    //         data => SendResponse(HttpStatusCode.OK, data: data),
    //         err => SendResponse(err)
    //     );
    // }
}