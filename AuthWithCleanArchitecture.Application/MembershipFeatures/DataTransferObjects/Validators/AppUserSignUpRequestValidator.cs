using FluentValidation;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects.Validators;

public class AppUserSignUpRequestValidator : AbstractValidator<AppUserSignUpRequest>
{
    public AppUserSignUpRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(6, 100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 100);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password);
    }
}