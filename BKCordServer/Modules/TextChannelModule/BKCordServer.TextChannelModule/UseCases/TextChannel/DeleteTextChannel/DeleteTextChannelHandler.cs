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

        var isUserHavePermission = await _mediator.Send(new IsUserHavePermissionQuery(userId, textChannel.ServerId, RolePermission.ManageChannels));

        if (!isUserHavePermission)
            throw new ForbiddenException("User doesn't have permission");

        textChannel.IsDeleted = true;
        textChannel.DeletedBy = userId;
        textChannel.DeletedAt = DateTime.UtcNow;

        var textMessages = await _dbContext.TextMessages.Where(m => m.ChannelId == textChannel.Id).ToListAsync();

        foreach (var textMessage in textMessages)
        {
            textMessage.IsDeleted = true;
            textMessage.DeletedBy = userId;
            textMessage.DeletedAt = DateTime.UtcNow;
        }

        _dbContext.TextChannels.Update(textChannel);
        _dbContext.TextMessages.UpdateRange(textMessages);

        await _dbContext.SaveChangesAsync();
    }
}
