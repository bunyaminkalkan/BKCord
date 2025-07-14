using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;

public sealed record CreateTextChannelCommand(Guid ServerId, string Name) : IRequest<Domain.Entities.TextChannel>;
