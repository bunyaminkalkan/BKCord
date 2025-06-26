using FluentValidation;

namespace BKCordServer.Modules.Identity.Application.Features.Quries.GetByEmail;
public class GetByEmailValidator : AbstractValidator<GetByEmailQuery>
{
    public GetByEmailValidator()
    {
        RuleFor(r => r.Email)
            .EmailAddress().WithMessage("Please enter a valid email address.");
    }
}
