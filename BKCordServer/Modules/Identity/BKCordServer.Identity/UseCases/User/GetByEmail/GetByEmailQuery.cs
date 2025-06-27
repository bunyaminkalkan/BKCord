using BKCordServer.Identity.DTOs;
using MediatR;

namespace BKCordServer.Identity.UseCases.User.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IRequest<UserDTO>;