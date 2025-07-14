using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.DeleteTextChannel;
public sealed record DeleteTextChannelCommand(Guid TextChannelId) : IRequest;
