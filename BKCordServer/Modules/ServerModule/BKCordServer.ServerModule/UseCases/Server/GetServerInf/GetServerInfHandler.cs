using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.GetServerInf;
public class GetServerInfHandler : IRequestHandler<GetServerInfQuery, ServerInfDTO>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public GetServerInfHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService serverAuthorizationService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task<ServerInfDTO> Handle(GetServerInfQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(userId, request.ServerId);

        var server = await _dbContext.Servers.FirstOrDefaultAsync(s => s.Id == request.ServerId)
            ?? throw new NotFoundException($"Server cannot be find with {request.ServerId} server id");

        return new ServerInfDTO(server);
    }
}
