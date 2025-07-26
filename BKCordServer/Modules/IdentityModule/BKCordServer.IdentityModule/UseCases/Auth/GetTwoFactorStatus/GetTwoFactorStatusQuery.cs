using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.GetTwoFactorStatus;
public sealed record GetTwoFactorStatusQuery() : IRequest<bool>;
