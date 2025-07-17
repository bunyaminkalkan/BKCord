using BKCordServer.ServerModule.Contracts;
using BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;
using Shared.Kernel.Services;

namespace BKCordServer.VoiceChannelModule.UseCases.CreateVoiceChannel;
public class CreateVoiceChannelHandler : IRequestHandler<CreateVoiceChannelCommand, VoiceChannel>
{
    private readonly AppVoiceChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public CreateVoiceChannelHandler(AppVoiceChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    public async Task<VoiceChannel> Handle(CreateVoiceChannelCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserHavePermissionQuery(userId, request.ServerId, RolePermission.ManageChannels));

        var voiceChannel = new VoiceChannel
        {
            ServerId = request.ServerId,
            CreatedBy = userId,
            Name = request.Name
        };

        _dbContext.VoiceChannels.Add(voiceChannel);
        await _dbContext.SaveChangesAsync();

        return voiceChannel;
    }
}
