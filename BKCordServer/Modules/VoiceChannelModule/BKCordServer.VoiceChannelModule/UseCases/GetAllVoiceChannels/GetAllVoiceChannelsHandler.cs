using BKCordServer.ServerModule.Contracts;
using BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Services;

namespace BKCordServer.VoiceChannelModule.UseCases.GetAllVoiceChannels;
public class GetAllVoiceChannelsHandler : IRequestHandler<GetAllVoiceChannelsQuery, IEnumerable<VoiceChannel>>
{
    private readonly AppVoiceChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public GetAllVoiceChannelsHandler(AppVoiceChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    public async Task<IEnumerable<VoiceChannel>> Handle(GetAllVoiceChannelsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserMemberTheServerQuery(userId, request.ServerId));

        return await _dbContext.VoiceChannels.Where(vc => vc.ServerId == request.ServerId).ToListAsync();
    }
}
