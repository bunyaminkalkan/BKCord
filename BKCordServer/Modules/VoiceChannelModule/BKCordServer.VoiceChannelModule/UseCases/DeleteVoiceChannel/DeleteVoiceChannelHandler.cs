using BKCordServer.ServerModule.Contracts;
using BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.VoiceChannelModule.UseCases.DeleteVoiceChannel;
public class DeleteVoiceChannelHandler : IRequestHandler<DeleteVoiceChannelCommand>
{
    private readonly AppVoiceChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public DeleteVoiceChannelHandler(AppVoiceChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    public async Task Handle(DeleteVoiceChannelCommand request, CancellationToken cancellationToken)
    {
        var voiceChannel = await _dbContext.VoiceChannels.FirstOrDefaultAsync(vc => vc.Id == request.VoiceChannelId)
                ?? throw new NotFoundException($"Voice Channel cannot find with {request.VoiceChannelId} voice channel id");

        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserHavePermissionQuery(userId, voiceChannel.ServerId, RolePermission.ManageChannels));

        voiceChannel.DeletedBy = userId;

        _dbContext.VoiceChannels.Remove(voiceChannel);

        await _dbContext.SaveChangesAsync();
    }
}
