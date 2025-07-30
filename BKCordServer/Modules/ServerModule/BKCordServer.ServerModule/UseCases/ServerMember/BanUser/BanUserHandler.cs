using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.BanUser;
public class BanUserHandler : IRequestHandler<BanUserCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public BanUserHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService serverAuthorizationService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(request.UserId, request.ServerId);
        await _serverAuthorizationService.ValidateUserHavePermissionByUserIdAndServerId(userId, request.ServerId, Contracts.RolePermission.BanMembers);

        var serverMember = await _dbContext.ServerMembers.FirstOrDefaultAsync(sm => sm.UserId == request.UserId && sm.ServerId == request.ServerId)
            ?? throw new NotFoundException($"Server member connot be find {request.UserId} user id and {request.ServerId} server id");

        var serverMembersHistory = await _dbContext.ServerMembersHistory.FirstOrDefaultAsync(smh => smh.UserId == request.UserId && smh.ServerId == request.ServerId)
            ?? throw new NotFoundException($"History connot be find with {request.ServerId} server id and {request.UserId} user id");

        serverMembersHistory.IsBanned = true;
        serverMembersHistory.Reason = request.Reason;
        serverMembersHistory.ActionedByUserId = userId;
        serverMembersHistory.UpdatedAt = DateTime.UtcNow;

        _dbContext.ServerMembers.Remove(serverMember);
        _dbContext.ServerMembersHistory.Update(serverMembersHistory);

        await _dbContext.SaveChangesAsync();
    }
}
