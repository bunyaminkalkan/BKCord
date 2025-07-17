using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.User.GetByEmail;
public sealed class GetByEmailHandler : IRequestHandler<GetByEmailQuery, UserDTO>
{
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public GetByEmailHandler(AppIdentityDbContext appIdentityDbContext)
    {
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<UserDTO> Handle(GetByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _appIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new NotFoundException($"User cannot find with {request.Email} email");

        return new UserDTO(user);
    }
}
