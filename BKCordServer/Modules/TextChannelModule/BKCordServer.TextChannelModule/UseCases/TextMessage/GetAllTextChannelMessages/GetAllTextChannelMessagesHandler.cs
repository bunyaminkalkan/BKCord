using BKCordServer.IdentityModule.Contracts;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
public class GetAllTextChannelMessagesHandler : IRequestHandler<GetAllTextChannelMessagesQuery, IEnumerable<TextMessageDTO>>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public GetAllTextChannelMessagesHandler(
        AppTextChannelDbContext dbContext,
        IHttpContextService httpContextService,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<TextMessageDTO>> Handle(GetAllTextChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var textChannel = await _dbContext.TextChannels.FirstOrDefaultAsync(tc => tc.Id == request.TextChannelId)
            ?? throw new NotFoundException($"Text Channel cannot find with {request.TextChannelId} text channel id");

        await _mediator.Send(new ValidateUserMemberTheServerQuery(userId, textChannel.ServerId));

        var textMessagesQuery = _dbContext.TextMessages.Where(m => m.ChannelId == textChannel.Id);

        if (request.Before.HasValue)
            textMessagesQuery = textMessagesQuery.Where(m => m.CreatedAt < request.Before.Value);

        var textMessages = await textMessagesQuery.OrderByDescending(m => m.CreatedAt).Take(request.PageSize).ToListAsync();

        var userIds = textMessages.Select(msg => msg.SenderUserId).Distinct().ToList();

        var userInfos = await _mediator.Send(new ListUserInfsQuery(userIds));

        var userInfoDict = userInfos.ToDictionary(user => user.Id, user => user);

        var textMessageDTOs = textMessages.Select(textMessage =>
            new TextMessageDTO(
                userInfoDict[textMessage.SenderUserId],
                textMessage,
                textMessage.CreatedAt != (textMessage.UpdatedAt ?? textMessage.CreatedAt))
            )
            .ToList();

        return textMessageDTOs;
    }
}
