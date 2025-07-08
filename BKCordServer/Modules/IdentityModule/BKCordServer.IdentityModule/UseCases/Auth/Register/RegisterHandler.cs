using BKCordServer.IdentityModule.Services;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.Auth.Register;

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
