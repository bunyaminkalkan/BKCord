using BKCordServer.Modules.Identity.Application.Services;
using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Domain.Repositories;

namespace BKCordServer.Modules.Identity.Infrastructure.Persistance.Services;
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
            throw new Exception("user not found");

        return user;
    }
}
