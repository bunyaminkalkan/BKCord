using MediatR;

namespace BKCordServer.Identity.UseCases.Auth.Register;

public sealed record RegisterCommand(
    string? Name,
    string? Middlename,
    string? Surname,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword
    ) : IRequest;
