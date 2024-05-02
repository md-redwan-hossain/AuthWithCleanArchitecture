using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Outcomes;
using AuthWithCleanArchitecture.Domain.AppUserAggregate;
using SharpOutcome;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures;

public interface IMembershipService
{
    Task<Outcome<AppUser, SignUpBadOutcome>> SignUpAsync(AppUserSignUpRequest dto);
    Task<Outcome<string, LoginBadOutcome>> LoginAsync(AppUserLoginRequest dto);
}