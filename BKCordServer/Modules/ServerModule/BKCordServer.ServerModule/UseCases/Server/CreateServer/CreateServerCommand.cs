using BKCordServer.ServerModule.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BKCordServer.ServerModule.UseCases.Server.CreateServer;
public sealed record CreateServerCommand(string Name, IFormFile Logo) : IRequest<ServerInfDTO>;
