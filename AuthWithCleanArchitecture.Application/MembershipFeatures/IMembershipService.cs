using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;
using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using SharpOutcome;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures;

public interface IMembershipService
{
    Task<ValueOutcome<AppUser, SignUpBadOutcome>> SignUpAsync(AppUserSignUpRequest dto);
    Task<ValueOutcome<string, LoginBadOutcome>> LoginAsync(AppUserLoginRequest dto);
    Task<ValueOutcome<AppUser, ProfileBadOutcome>> ReadProfileAsync(AppUserId id);

    Task<ValueOutcome<AppUser, ProfileBadOutcome>> UpdateProfileAsync(AppUserId id,
        AppUserProfileUpdateRequest dto);
}