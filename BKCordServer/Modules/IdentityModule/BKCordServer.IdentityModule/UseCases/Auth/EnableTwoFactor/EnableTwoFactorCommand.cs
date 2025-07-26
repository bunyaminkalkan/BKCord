using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.EnableTwoFactor;
public sealed record EnableTwoFactorCommand(string VerificationCode) : IRequest<string[]>;
