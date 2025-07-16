using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.DeleteTextChannel;
public class DeleteTextChannelHandler : IRequestHandler<DeleteTextChannelCommand>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public DeleteTextChannelHandler(AppTextChannelDbContext dbContext, IHttpContextService httpContextService, IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task Handle(DeleteTextChannelCommand request, CancellationToken cancellationToken)
    {
        var textChannel = await _dbContext.TextChannels.FirstOrDefaultAsync(tc => tc.Id == request.TextChannelId)
            ?? throw new NotFoundException($"Text Channel cannot find with {request.TextChannelId} text channel id");

        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserHavePermissionQuery(userId, textChannel.ServerId, RolePermission.ManageChannels));

        var textMessages = await _dbContext.TextMessages.Where(m => m.ChannelId == textChannel.Id).ToListAsync();

        foreach (var textMessage in textMessages)
        {
            textMessage.DeletedBy = userId;
        }

        textChannel.DeletedBy = userId;

        _dbContext.TextChannels.Remove(textChannel);
        _dbContext.TextMessages.RemoveRange(textMessages);

        await _dbContext.SaveChangesAsync();
    }
}
