using BKCordServer.TextChannelModule.DTOs;
using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
public sealed record GetAllTextChannelMessagesQuery(
    Guid TextChannelId,
    DateTime? Before,
    int PageSize = 20) : IRequest<IEnumerable<TextMessageDTO>>;
