using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Images;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.CreateServer;
public class CreateServerHandler : IRequestHandler<CreateServerCommand>
{
    private readonly IServerService _serverService;
    private readonly IImageService _imageService;
    private readonly IHttpContextService _httpContextService;

    public CreateServerHandler(IServerService serverService, IImageService imageService, IHttpContextService httpContextService)
    {
        _serverService = serverService;
        _imageService = imageService;
        _httpContextService = httpContextService;
    }

    public async Task Handle(CreateServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var logoUrl = await _imageService.UploadSingleImageAsync(request.Logo, "servers");

        await _serverService.CreateAsync(userId, request.Name, logoUrl);
    }
}
