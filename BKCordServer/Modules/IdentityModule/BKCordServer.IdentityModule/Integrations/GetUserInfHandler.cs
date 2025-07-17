using BKCordServer.IdentityModule.Contracts;
using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.Integrations;
public class GetUserInfHandler : IRequestHandler<GetUserInfQuery, UserInfDTO>
{
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public GetUserInfHandler(AppIdentityDbContext appIdentityDbContext)
    {
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<UserInfDTO> Handle(GetUserInfQuery request, CancellationToken cancellationToken)
    {
        var user = await _appIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId)
            ?? throw new NotFoundException($"User cannot find with {request.UserId} user id");

        return new UserInfDTO(user.Id, user.UserName, user.AvatarUrl);
    }
}
