using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.GetServerRoles;
public sealed class GetServerRolesHandler : IRequestHandler<GetServerRolesQuery, IEnumerable<Domain.Entities.Role>>
{
    private readonly IRoleService _roleService;

    public GetServerRolesHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IEnumerable<Domain.Entities.Role>> Handle(GetServerRolesQuery request, CancellationToken cancellationToken) =>
        await _roleService.GetAllByServerIdAsync(request.ServerId);
}
