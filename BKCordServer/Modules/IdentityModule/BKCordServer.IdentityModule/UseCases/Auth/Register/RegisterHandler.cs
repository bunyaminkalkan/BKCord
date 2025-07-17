using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public RegisterHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Middlename = request.Middlename,
            Surname = request.Surname,
            IsPrivateAccount = false,
            EmailConfirmed = false,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Register failed: {errors}");
        }
    }
}
