using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Services;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
public class UpdateTextChannelHandler : IRequestHandler<UpdateTextChannelCommand, Domain.Entities.TextChannel>
{
    private readonly ITextChannelService _textChannelService;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public UpdateTextChannelHandler(ITextChannelService textChannelService, IHttpContextService httpContextService, IMediator mediator)
    {
        _textChannelService = textChannelService;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<Domain.Entities.TextChannel> Handle(UpdateTextChannelCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var textChannel = await _textChannelService.GetByIdAsync(request.ChannelId);

        var query = new IsUserHavePermissionQuery(userId, textChannel.ServerId, RolePermission.ManageChannels);
        var isUserHavePermission = await _mediator.Send(query);

        if (!isUserHavePermission)
            throw new ForbiddenException("User doesn't have permission");

        return await _textChannelService.UpdateAsync(userId, request, textChannel);
    }
}
