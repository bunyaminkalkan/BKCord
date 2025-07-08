using BKCordServer.IdentityModule.DTOs;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<JwtResponse>;
