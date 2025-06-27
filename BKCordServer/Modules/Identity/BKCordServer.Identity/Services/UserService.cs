using BKCordServer.Identity.Domain.Entities;
using BKCordServer.Identity.Repositories;
using Shared.Kernel.Exceptions;

namespace BKCordServer.Identity.Services;

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
}
