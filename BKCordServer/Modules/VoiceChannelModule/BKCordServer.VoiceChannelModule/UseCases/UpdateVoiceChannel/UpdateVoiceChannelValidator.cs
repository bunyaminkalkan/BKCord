using FluentValidation;

namespace BKCordServer.VoiceChannelModule.UseCases.UpdateVoiceChannel;
public class UpdateVoiceChannelValidator : AbstractValidator<UpdateVoiceChannelCommand>
{
    public UpdateVoiceChannelValidator()
    {
        RuleFor(vc => vc.Name)
            .NotEmpty()
            .WithMessage("Channel name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Channel name cannot exceed 100 characters.");
    }
}
