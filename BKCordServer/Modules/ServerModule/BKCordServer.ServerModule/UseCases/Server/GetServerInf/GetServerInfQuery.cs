using BKCordServer.ServerModule.DTOs;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Server.GetServerInf;

public sealed record GetServerInfQuery(Guid ServerId) : IRequest<ServerInfDTO>;
