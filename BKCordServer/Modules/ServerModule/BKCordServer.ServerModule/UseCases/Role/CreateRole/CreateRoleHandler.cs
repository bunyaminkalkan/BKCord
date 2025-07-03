using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public class CreateRoleHandler : IRequestHandler<CreateRoleCommand>
{
    private readonly IRoleService _roleService;

    public CreateRoleHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleService.ValidateRoleExist(request.ServerId, request.Name);
        await _roleService.CreateAsync(request);
    }
}
