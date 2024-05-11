using FluentValidation;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Validators;

public class AppUserProfileUpdateRequestValidator : AbstractValidator<AppUserProfileUpdateRequest>
{
    public AppUserProfileUpdateRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .When(x => string.IsNullOrEmpty(x.Email) is false);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.PhoneNumber) is false);
    }
}