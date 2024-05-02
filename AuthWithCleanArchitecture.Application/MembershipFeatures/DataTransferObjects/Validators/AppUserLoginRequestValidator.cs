using FluentValidation;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Validators;

public class AppUserLoginRequestValidator : AbstractValidator<AppUserLoginRequest>
{
    public AppUserLoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(6, 100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 100);
    }
}