using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
public sealed record UpdateTextChannelCommand(Guid ChannelId, string Name) : IRequest<Domain.Entities.TextChannel>;
