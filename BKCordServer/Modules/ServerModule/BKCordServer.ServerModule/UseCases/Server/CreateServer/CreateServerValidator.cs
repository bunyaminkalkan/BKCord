using FluentValidation;

namespace BKCordServer.ServerModule.UseCases.Server.CreateServer;
public class CreateServerValidator : AbstractValidator<CreateServerCommand>
{
    public CreateServerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Server name cannot be empty")
            .MaximumLength(100).WithMessage("Server name cannot exceed 100 characters.");

        RuleFor(x => x.Logo)
            .NotEmpty().WithMessage("Logo cannot be empty");
    }
}
