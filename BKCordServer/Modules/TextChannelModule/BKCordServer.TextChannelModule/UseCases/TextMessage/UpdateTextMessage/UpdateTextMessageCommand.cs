using BKCordServer.TextChannelModule.DTOs;
using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.UpdateTextMessage;
public sealed record UpdateTextMessageCommand(Guid TextMessageId, string NewContent) : IRequest<TextMessageDTO>;
