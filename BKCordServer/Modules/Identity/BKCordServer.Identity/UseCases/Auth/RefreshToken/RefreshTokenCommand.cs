using BKCordServer.Identity.DTOs;
using MediatR;

namespace BKCordServer.Identity.UseCases.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<JwtResponse>;
