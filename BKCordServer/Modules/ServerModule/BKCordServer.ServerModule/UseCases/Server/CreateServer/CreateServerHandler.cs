using BKCordServer.ServerModule.DTOs;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Images;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.CreateServer;
public class CreateServerHandler : IRequestHandler<CreateServerCommand, ServerInfDTO>
{
    private readonly IServerService _serverService;
    private readonly IImageService _imageService;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerMemberService _serverMemberService;
    private readonly IServerMembersHistoryService _serverMembersHistoryService;

    public CreateServerHandler(
        IServerService serverService,
        IImageService imageService,
        IHttpContextService httpContextService,
        IServerMemberService serverMemberService,
        IServerMembersHistoryService serverMembersHistoryService
        )
    {
        _serverService = serverService;
        _imageService = imageService;
        _httpContextService = httpContextService;
        _serverMemberService = serverMemberService;
        _serverMembersHistoryService = serverMembersHistoryService;
    }

    public async Task<ServerInfDTO> Handle(CreateServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var logoUrl = await _imageService.UploadSingleImageAsync(request.Logo, "servers");

        var server = await _serverService.CreateAsync(userId, request.Name, logoUrl);

        await _serverMemberService.JoinServerAsync(userId, server.Id);
        await _serverMembersHistoryService.JoinServerAsync(userId, server.Id);

        return new ServerInfDTO(server);
    }
}
