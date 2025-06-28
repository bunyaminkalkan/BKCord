using BKCordServer.Identity.DTOs;
using MediatR;

namespace BKCordServer.Identity.UseCases.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<JwtResponse>;
