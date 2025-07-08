using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.UpdateRole;
public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand>
{
    private readonly IRoleService _roleService;

    public UpdateRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleService.UpdateAsync(request);
    }
}
