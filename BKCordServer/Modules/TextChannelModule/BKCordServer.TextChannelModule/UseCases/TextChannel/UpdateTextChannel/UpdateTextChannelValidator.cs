using FluentValidation;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
public class UpdateTextChannelValidator : AbstractValidator<UpdateTextChannelCommand>
{
    public UpdateTextChannelValidator()
    {
        RuleFor(tc => tc.Name)
            .NotEmpty()
            .WithMessage("Channel name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Channel name cannot exceed 100 characters.");
    }
}
