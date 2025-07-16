using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServersByMemberUser;
public class GetServersByMemberUserHandler : IRequestHandler<GetServersByMemberUserQuery, IEnumerable<ServerDTO>>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;

    public GetServersByMemberUserHandler(AppServerDbContext dbContext, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
    }

    public async Task<IEnumerable<ServerDTO>> Handle(GetServersByMemberUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var serverIds = await _dbContext.ServerMembers.Where(sm => sm.UserId == userId).Select(sm => sm.ServerId).ToListAsync();

        var servers = await _dbContext.Servers.Where(s => serverIds.Contains(s.Id)).ToListAsync();

        return servers.Select(server => new ServerDTO(server));
    }
}
