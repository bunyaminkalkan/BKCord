using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
public class UpdateTextChannelHandler : IRequestHandler<UpdateTextChannelCommand, Domain.Entities.TextChannel>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public UpdateTextChannelHandler(AppTextChannelDbContext dbContext, IHttpContextService httpContextService, IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<Domain.Entities.TextChannel> Handle(UpdateTextChannelCommand request, CancellationToken cancellationToken)
    {
        var textChannel = await _dbContext.TextChannels.FirstOrDefaultAsync(tc => tc.Id == request.TextChannelId)
            ?? throw new NotFoundException($"Text Channel cannot find with {request.TextChannelId} text channel id");

        var userId = _httpContextService.GetUserId();

        await _mediator.Send(new ValidateUserHavePermissionQuery(userId, textChannel.ServerId, RolePermission.ManageChannels));

        textChannel.Name = request.Name;
        textChannel.UpdatedBy = userId;

        _dbContext.TextChannels.Update(textChannel);
        await _dbContext.SaveChangesAsync();

        return textChannel;
    }
}
