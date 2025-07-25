using BKCordServer.IdentityModule.DTOs;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.ForgotPassword;
public sealed record ForgotPasswordCommand(string Email) : IRequest<MailResponse>;
