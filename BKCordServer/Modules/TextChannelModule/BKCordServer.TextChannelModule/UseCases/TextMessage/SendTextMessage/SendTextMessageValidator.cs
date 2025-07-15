using FluentValidation;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
public class SendTextMessageValidator : AbstractValidator<SendTextMessageCommand>
{
    public SendTextMessageValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Sender user ID must not be empty.");

        RuleFor(x => x.TextChannelId)
            .NotEmpty().WithMessage("Text channel ID must not be empty.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message content must not be empty.")
            .MaximumLength(2000).WithMessage("Message content must not exceed 2000 characters.");
    }
}

