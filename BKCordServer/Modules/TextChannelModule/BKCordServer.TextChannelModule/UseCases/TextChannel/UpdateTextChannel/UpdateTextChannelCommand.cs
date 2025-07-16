using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
public sealed record UpdateTextChannelCommand(Guid TextChannelId, string Name) : IRequest<Domain.Entities.TextChannel>;
