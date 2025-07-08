using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.User.GetByEmail;

public class GetByEmailValidator : AbstractValidator<GetByEmailQuery>
{
    public GetByEmailValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress().WithMessage("Please enter a valid email address.");
    }
}
