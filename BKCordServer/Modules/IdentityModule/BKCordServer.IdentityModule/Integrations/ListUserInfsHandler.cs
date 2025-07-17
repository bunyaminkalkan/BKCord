using BKCordServer.IdentityModule.Contracts;
using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.IdentityModule.Integrations;
public sealed class ListUserInfsHandler : IRequestHandler<ListUserInfsQuery, IEnumerable<UserInfDTO>>
{
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public ListUserInfsHandler(AppIdentityDbContext appIdentityDbContext)
    {
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<IEnumerable<UserInfDTO>> Handle(ListUserInfsQuery request, CancellationToken cancellationToken)
    {
        var users = await _appIdentityDbContext.Users.Where(u => request.UserIds.Contains(u.Id)).ToListAsync();

        return users.Select(u => new UserInfDTO(u.Id, u.UserName, u.AvatarUrl));
    }
}
