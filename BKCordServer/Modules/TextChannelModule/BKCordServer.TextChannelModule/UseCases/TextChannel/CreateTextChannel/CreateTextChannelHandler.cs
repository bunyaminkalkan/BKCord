using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
public class CreateTextChannelHandler : IRequestHandler<CreateTextChannelCommand, Domain.Entities.TextChannel>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;

    public CreateTextChannelHandler(AppTextChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
    }

    public async Task<Domain.Entities.TextChannel> Handle(CreateTextChannelCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var isUserHavePermission = await _mediator.Send(new IsUserHavePermissionQuery(userId, request.ServerId, RolePermission.ManageChannels));

        if (!isUserHavePermission)
            throw new ForbiddenException("User doesn't have permission");

        var textChannel = new Domain.Entities.TextChannel
        {
            ServerId = request.ServerId,
            CreatedBy = userId,
            Name = request.Name
        };

        _dbContext.TextChannels.Add(textChannel);
        await _dbContext.SaveChangesAsync();

        return textChannel;
    }
}
