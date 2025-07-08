using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.IdentityModule.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task AddAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task UpdateAsync(User user) =>
        await _userManager.UpdateAsync(user);

    public async Task DeleteAsync(User user) =>
        await _userManager.DeleteAsync(user);

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user is null ? null : user;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user is null ? null : user;
    }

    public async Task<IEnumerable<User>> GetAllByIdsAsync(IEnumerable<Guid> userIds) =>
        await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

}
