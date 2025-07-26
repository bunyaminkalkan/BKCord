using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.GenerateTwoFactorRecoveryCodes;
public sealed record GenerateTwoFactorRecoveryCodesCommand() : IRequest<string[]>;
