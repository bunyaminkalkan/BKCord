using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.DisableTwoFactor;
public sealed record DisableTwoFactorCommand() : IRequest;
