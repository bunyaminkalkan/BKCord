using BKCordServer.IdentityModule.Contracts;
using BKCordServer.ServerModule.DTOs;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServerUsers;
public sealed class GetServerUsersHandler : IRequestHandler<GetServerUsersQuery, IEnumerable<RoleBasedServerUsersDTO>>
{
    private readonly IRoleService _roleService;
    private readonly IRoleMemberService _roleMemberService;
    private readonly IMediator _mediator;

    public GetServerUsersHandler(IRoleService roleService, IRoleMemberService roleMemberService, IMediator mediator)
    {
        _roleService = roleService;
        _roleMemberService = roleMemberService;
        _mediator = mediator;
    }

    public async Task<IEnumerable<RoleBasedServerUsersDTO>> Handle(GetServerUsersQuery request, CancellationToken cancellationToken)
    {
        var roleBasedServerUsersDTOs = new List<RoleBasedServerUsersDTO>();
        var addedUserIds = new List<Guid>();

        var roles = await _roleService.GetAllByServerIdAsync(request.ServerId);

        foreach (var role in roles)
        {
            var userIds = (await _roleMemberService.GetUserIdsByRoleIdAsync(role.Id))
                .Except(addedUserIds)
                .ToList();

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
