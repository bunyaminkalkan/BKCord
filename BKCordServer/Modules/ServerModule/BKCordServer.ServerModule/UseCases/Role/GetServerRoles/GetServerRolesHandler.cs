using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.GetServerRoles;
public sealed class GetServerRolesHandler : IRequestHandler<GetServerRolesQuery, IEnumerable<Domain.Entities.Role>>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IServerAuthorizationService _serverAuthorizationService;
    private readonly IHttpContextService _httpContextService;

    public GetServerRolesHandler(AppServerDbContext dbContext, IServerAuthorizationService serverAuthorizationService, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _serverAuthorizationService = serverAuthorizationService;
        _httpContextService = httpContextService;
    }

    public async Task<IEnumerable<Domain.Entities.Role>> Handle(GetServerRolesQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(userId, request.ServerId);

        return await _dbContext.Roles.Where(r => r.ServerId == request.ServerId).OrderByDescending(r => r.Hierarchy).ToListAsync();
    }

}
