using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;
using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using SharpOutcome;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures;

public interface IMembershipService
{
    Task<Outcome<AppUser, SignUpBadOutcome>> SignUpAsync(AppUserSignUpRequest dto);
    Task<Outcome<string, LoginBadOutcome>> LoginAsync(AppUserLoginRequest dto);
    Task<Outcome<AppUser, ProfileBadOutcome>> ProfileAsync(AppUserId id);
}