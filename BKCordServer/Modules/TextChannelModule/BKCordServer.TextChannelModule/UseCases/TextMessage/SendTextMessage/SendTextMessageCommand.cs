using BKCordServer.TextChannelModule.DTOs;
using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
public record SendTextMessageCommand(Guid UserId, Guid TextChannelId, string Content) : IRequest<TextMessageDTO>;
