using BKCordServer.IdentityModule.DTOs;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.Login;

public sealed record LoginCommand(string Email, string Password, string? TwoFactorCode) : IRequest<JwtResponse>;
