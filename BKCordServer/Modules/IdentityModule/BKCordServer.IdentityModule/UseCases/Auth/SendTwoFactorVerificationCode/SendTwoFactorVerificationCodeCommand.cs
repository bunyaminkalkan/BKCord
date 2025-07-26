using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.SendTwoFactorVerificationCode;
public sealed record SendTwoFactorVerificationCodeCommand() : IRequest;
