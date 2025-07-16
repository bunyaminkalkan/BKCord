using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.GetAllTextChannels;
public class GetAllTextChannelsHandler : IRequestHandler<GetAllTextChannelsQuery, IEnumerable<Domain.Entities.TextChannel>>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;

    public GetAllTextChannelsHandler(AppTextChannelDbContext dbContext, IHttpContextService httpContextService, IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<Domain.Entities.TextChannel>> Handle(GetAllTextChannelsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var isUserMemberTheServer = await _mediator.Send(new IsUserMemberTheServerQuery(userId, request.ServerId));

        if (!isUserMemberTheServer)
            throw new BadRequestException("User has not joined the server");

        return await _dbContext.TextChannels.Where(tc => tc.ServerId == request.ServerId).ToListAsync();
    }
}
