using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.DeleteTextMessage;
public sealed record DeleteTextMessageCommand(Guid TextMessageId) : IRequest;
