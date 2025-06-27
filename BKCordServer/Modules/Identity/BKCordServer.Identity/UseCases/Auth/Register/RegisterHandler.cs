using BKCordServer.Identity.Services;
using MediatR;

namespace BKCordServer.Identity.UseCases.Auth.Register;

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
