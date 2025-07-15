using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Repositories;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            throw new NotFoundException("user not found");

        return user;
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new NotFoundException($"User was not found with {userId} user id");

        return user;
    }

    public async Task<IEnumerable<User>> GetAllByIdsAsync(IEnumerable<Guid> userIds) =>
        await _userRepository.GetAllByIdsAsync(userIds);
}
