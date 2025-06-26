using MediatR;

namespace BKCordServer.Modules.Identity.Application.Features.Commands.Register;

public sealed record RegisterCommand(
    string? Name,
    string? Middlename,
    string? Surname,
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword
    ) : IRequest;
