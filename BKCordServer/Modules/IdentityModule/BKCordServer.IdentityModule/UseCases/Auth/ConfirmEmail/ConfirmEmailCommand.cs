using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.ConfirmEmail;
public sealed record ConfirmEmailCommand(Guid TokenId, string Token) : IRequest;
