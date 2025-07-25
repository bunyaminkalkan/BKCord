using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.ResendEmailConfirmation;
public sealed record ResendEmailConfirmationCommand(string Email) : IRequest;
