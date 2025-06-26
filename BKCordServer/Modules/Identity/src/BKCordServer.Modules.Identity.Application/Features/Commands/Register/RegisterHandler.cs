using BKCordServer.Modules.Identity.Application.Services;
using MediatR;

namespace BKCordServer.Modules.Identity.Application.Features.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly IAuthService _authService;

    public RegisterHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken) =>
        await _authService.RegisterAsync(request);
}
