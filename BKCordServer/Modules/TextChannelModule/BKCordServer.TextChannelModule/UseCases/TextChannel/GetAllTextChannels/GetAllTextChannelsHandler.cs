using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Services;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.GetAllTextChannels;
public class GetAllTextChannelsHandler : IRequestHandler<GetAllTextChannelsQuery, IEnumerable<Domain.Entities.TextChannel>>
{
    private readonly ITextChannelService _textChannelService;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public GetAllTextChannelsHandler(ITextChannelService textChannelService, IHttpContextService httpContextService, IMediator mediator)
    {
        _textChannelService = textChannelService;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<Domain.Entities.TextChannel>> Handle(GetAllTextChannelsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var query = new IsUserMemberTheServerQuery(userId, request.ServerId);
        var isUserMemberTheServer = await _mediator.Send(query);

        if (!isUserMemberTheServer)
            throw new BadRequestException("User has not joined the server");

        return await _textChannelService.GetAllByServerIdAsync(request.ServerId);
    }
}
