using BKCordServer.IdentityModule.Contracts;
using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServerUsers;
public sealed class GetServerUsersHandler : IRequestHandler<GetServerUsersQuery, IEnumerable<RoleBasedServerUsersDTO>>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public GetServerUsersHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IMediator mediator, IServerAuthorizationService serverAuthorizationService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _mediator = mediator;
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task<IEnumerable<RoleBasedServerUsersDTO>> Handle(GetServerUsersQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(userId, request.ServerId);

        var roleBasedServerUsersDTOs = new List<RoleBasedServerUsersDTO>();
        var addedUserIds = new List<Guid>();

        var roles = await _dbContext.Roles.Where(r => r.ServerId == request.ServerId).OrderByDescending(r => r.Hierarchy).ToListAsync();

        foreach (var role in roles)
        {
            var userIds = (await _dbContext.RoleMembers.Where(rm => rm.RoleId == role.Id).Select(rm => rm.UserId).ToListAsync())
                .Except(addedUserIds)
                .ToList();

            if (userIds.Count == 0)
                continue;

            addedUserIds.AddRange(userIds);

            var userInfs = await _mediator.Send(new ListUserInfsQuery(userIds));

            var roleDTO = new RoleDTO { Name = role.Name, Color = role.Color, Hierarchy = role.Hierarchy };

            var roleBasedServerUsersDTO = new RoleBasedServerUsersDTO
            {
                Role = roleDTO,
                Users = userInfs
            };

            roleBasedServerUsersDTOs.Add(roleBasedServerUsersDTO);
        }

        return roleBasedServerUsersDTOs;
    }
}
