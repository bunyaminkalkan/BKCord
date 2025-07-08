using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Interfaces;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Services.Classes;
public class RoleMemberService : IRoleMemberService
{
    private readonly IRoleMemberRepository _roleMemberRepository;

    public RoleMemberService(IRoleMemberRepository roleMemberRepository)
    {
        _roleMemberRepository = roleMemberRepository;
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        var roleMember = new RoleMember
        {
            UserId = userId,
            RoleId = roleId
        };

        await _roleMemberRepository.AddAsync(roleMember);
    }

    public async Task DeleteRoleFromUserAsync(Guid userId, Guid roleId)
    {
        var roleMember = await _roleMemberRepository.GetByUserIdRoleIdAndServerIdAsync(userId, roleId);

        if (roleMember == null)
            throw new NotFoundException($"Role Member cannot be find with {userId} user id and {roleId} role id");

        await _roleMemberRepository.DeleteAsync(roleMember);
    }

    public async Task DeleteAllMembersAsync(Guid roleId) =>
        await _roleMemberRepository.DeleteAllMembersAsync(roleId);
}
