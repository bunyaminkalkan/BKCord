using FluentValidation;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
public class CreateTextChannelValidator : AbstractValidator<CreateTextChannelCommand>
{
    public CreateTextChannelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Channel name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Channel name cannot exceed 100 characters.");

        RuleFor(x => x.ServerId)
            .NotEmpty()
            .WithMessage("Server ID cannot be empty.");
    }
}
