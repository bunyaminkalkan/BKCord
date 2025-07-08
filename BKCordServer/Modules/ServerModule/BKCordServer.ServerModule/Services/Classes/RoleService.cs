using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Interfaces;
using BKCordServer.ServerModule.UseCases.Role.CreateRole;
using BKCordServer.ServerModule.UseCases.Role.DeleteRole;
using BKCordServer.ServerModule.UseCases.Role.UpdateRole;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Services.Classes;
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task CreateAsync(CreateRoleCommand request)
    {
        var role = new Role
        {
            ServerId = request.ServerId,
            Name = request.Name,
            Color = request.Color,
            Hierarchy = request.Hierarchy,
            RolePermissions = request.RolePermissions
        };

        await _roleRepository.AddAsync(role);
    }

    public async Task UpdateAsync(UpdateRoleCommand request)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);

        if (role == null)
            throw new NotFoundException($"Role cannot be find with {request.RoleId} role id");

        role.Name = request.Name;
        role.Color = request.Color;
        role.Hierarchy = request.Hierarchy;
        role.RolePermissions = request.RolePermissions;

        await _roleRepository.UpdateAsync(role);
    }

    public async Task DeleteAsync(DeleteRoleCommand request)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId);

        if (role == null)
            throw new NotFoundException($"Role cannot be find with {request.RoleId} role id");

        await _roleRepository.DeleteAsync(role);
    }


    public async Task<Role> GetByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetByIdAsync(id);

        if (role == null)
            throw new NotFoundException($"Role cannot be find with {id} role id");

        return role;
    }

    public async Task<IEnumerable<Role>> GetAllByIdsAsync(IEnumerable<Guid> ids) =>
        await _roleRepository.GetAllByIdsAsync(ids);

    public async Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId) =>
        await _roleRepository.GetAllByServerIdAsync(serverId);

    public async Task ValidateRoleExist(Guid serverId, string name)
    {
        var role = await _roleRepository.GetByServerIdAndNameAsync(serverId, name);
        if (role != null)
            throw new BadRequestException($"Given '{name}' role already exist");
    }
}
