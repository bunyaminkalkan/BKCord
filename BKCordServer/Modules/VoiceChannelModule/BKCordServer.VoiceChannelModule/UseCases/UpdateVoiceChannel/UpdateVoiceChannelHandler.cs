using BKCordServer.ServerModule.Contracts;
using BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.VoiceChannelModule.UseCases.UpdateVoiceChannel;
public class UpdateVoiceChannelHandler : IRequestHandler<UpdateVoiceChannelCommand, VoiceChannel>
{
    private readonly AppVoiceChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public UpdateVoiceChannelHandler(AppVoiceChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    public async Task<VoiceChannel> Handle(UpdateVoiceChannelCommand request, CancellationToken cancellationToken)
    {
        var voiceChannelId = _httpContextService.GetIdFromRoute("VoiceChannelId");

        var voiceChannel = await _dbContext.VoiceChannels.FirstOrDefaultAsync(tc => tc.Id == voiceChannelId)
            ?? throw new NotFoundException($"Voice Channel cannot find with {voiceChannelId} text channel id");

        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserHavePermissionQuery(userId, voiceChannel.ServerId, RolePermission.ManageChannels));

        voiceChannel.Name = request.Name;
        voiceChannel.UpdatedBy = userId;

        _dbContext.VoiceChannels.Update(voiceChannel);
        await _dbContext.SaveChangesAsync();

        return voiceChannel;
    }
}
