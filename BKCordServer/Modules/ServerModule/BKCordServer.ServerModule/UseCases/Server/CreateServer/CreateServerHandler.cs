using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.DTOs;
using MediatR;
using Shared.Kernel.Images;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.CreateServer;
public class CreateServerHandler : IRequestHandler<CreateServerCommand, ServerInfDTO>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IImageService _imageService;
    private readonly IHttpContextService _httpContextService;

    public CreateServerHandler(AppServerDbContext dbContext, IImageService imageService, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _imageService = imageService;
        _httpContextService = httpContextService;
    }

    public async Task<ServerInfDTO> Handle(CreateServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var logoUrl = await _imageService.UploadSingleImageAsync(request.Logo, "servers");

        var server = new Domain.Entities.Server
        {
            OwnerId = userId,
            Name = request.Name,
            LogoUrl = logoUrl,
            InviteCode = GenerateInviteCode(),
        };

        var serverMember = new Domain.Entities.ServerMember
        {
            UserId = userId,
            ServerId = server.Id
        };

        var serverMembersHistory = new ServerMemberHistory
        {
            UserId = userId,
            ServerId = server.Id
        };

        _dbContext.Servers.Add(server);
        _dbContext.ServerMembers.Add(serverMember);
        _dbContext.ServerMembersHistory.Add(serverMembersHistory);

        await _dbContext.SaveChangesAsync();
        return new ServerInfDTO(server);
    }

    private string GenerateInviteCode(int length = 15)
    {
        var guid = Guid.NewGuid();
        var base64 = Convert.ToBase64String(guid.ToByteArray());

        // Base64 string'i URL-safe hale getir (isteğe bağlı)
        var code = base64.Replace("+", "")
                         .Replace("/", "")
                         .Replace("=", "");

        return code.Substring(0, Math.Min(length, code.Length));
    }
}
