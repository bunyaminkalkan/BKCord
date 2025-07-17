using FluentValidation;

namespace BKCordServer.VoiceChannelModule.UseCases.CreateVoiceChannel;
public class CreateVoiceChannelValidator : AbstractValidator<CreateVoiceChannelCommand>
{
    public CreateVoiceChannelValidator()
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
