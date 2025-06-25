using BKCordServer.Modules.Identity.Application.DTOs;
using MediatR;

namespace BKCordServer.Modules.Identity.Application.Features.Quries.GetByEmail;

public sealed record GetByEmailQuery(string Email) : IRequest<UserDTO>;