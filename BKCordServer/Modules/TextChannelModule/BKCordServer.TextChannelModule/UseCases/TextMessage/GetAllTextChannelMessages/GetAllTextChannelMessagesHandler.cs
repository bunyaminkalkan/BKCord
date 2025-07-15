using BKCordServer.IdentityModule.Contracts;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.DTOs;
using BKCordServer.TextChannelModule.Services;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
public class GetAllTextChannelMessagesHandler : IRequestHandler<GetAllTextChannelMessagesQuery, IEnumerable<TextMessageDTO>>
{
    private readonly ITextMessageService _textMessageService;
    private readonly ITextChannelService _textChannelService;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public GetAllTextChannelMessagesHandler(ITextMessageService textMessageService, ITextChannelService textChannelService, IHttpContextService httpContextService, IMediator mediator)
    {
        _textMessageService = textMessageService;
        _textChannelService = textChannelService;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<TextMessageDTO>> Handle(GetAllTextChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var textChannel = await _textChannelService.GetByIdAsync(request.TextChannelId);

        var isUserMemberTheServer = await _mediator.Send(new IsUserMemberTheServerQuery(userId, textChannel.ServerId));

        if (!isUserMemberTheServer)
            throw new BadRequestException("User has not joined the server");

        var textMessages = await _textMessageService.GetAllByChannelIdAsync(textChannel.Id);

        var userIds = textMessages.Select(msg => msg.SenderUserId).Distinct().ToList();

        var userInfos = await _mediator.Send(new ListUserInfsQuery(userIds));

        var userInfoDict = userInfos.ToDictionary(user => user.Id, user => user);

        var textMessageDTOs = textMessages.Select(textMessage =>
            new TextMessageDTO(userInfoDict[textMessage.SenderUserId], textMessage.Content))
            .ToList();

        return textMessageDTOs;
    }
}
