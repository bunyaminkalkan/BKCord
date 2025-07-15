using FluentValidation;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.UpdateTextMessage;
public class UpdateTextMessageValidator : AbstractValidator<UpdateTextMessageCommand>
{
    public UpdateTextMessageValidator()
    {
        RuleFor(x => x.TextMessageId)
            .NotEmpty().WithMessage("Text Message ID must not be empty");

        RuleFor(x => x.NewContent)
            .NotEmpty().WithMessage("Message content must not be empty")
            .MaximumLength(2000).WithMessage("Message content must not exceed 2000 characters");
    }
}
