using BKCordServer.IdentityModule.DTOs;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.User.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IRequest<UserDTO>;