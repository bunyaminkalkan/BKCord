using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.ResetPassword;
public sealed record ResetPasswordCommand(
    Guid TokenId,
    string Token,
    string NewPassword,
    string ConfirmNewPassword) : IRequest;