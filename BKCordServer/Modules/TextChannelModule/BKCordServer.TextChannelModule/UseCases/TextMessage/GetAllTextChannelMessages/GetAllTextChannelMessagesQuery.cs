using BKCordServer.TextChannelModule.DTOs;
using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
public sealed record GetAllTextChannelMessagesQuery(Guid TextChannelId) : IRequest<IEnumerable<TextMessageDTO>>;
