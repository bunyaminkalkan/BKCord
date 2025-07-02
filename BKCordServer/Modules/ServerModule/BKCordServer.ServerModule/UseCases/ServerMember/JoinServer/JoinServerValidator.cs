using FluentValidation;

namespace BKCordServer.ServerModule.UseCases.ServerMember.JoinServer;
public class JoinServerValidator : AbstractValidator<JoinServerCommand>
{
    public JoinServerValidator()
    {
        RuleFor(js => js.InviteCode)
            .NotEmpty().WithMessage("InviteCode connot be empty");
    }
}
