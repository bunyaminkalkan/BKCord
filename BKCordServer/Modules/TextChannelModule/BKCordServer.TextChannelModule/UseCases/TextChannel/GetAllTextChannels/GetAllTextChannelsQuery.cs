using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextChannel.GetAllTextChannels;
public sealed record GetAllTextChannelsQuery(Guid ServerId) : IRequest<IEnumerable<Domain.Entities.TextChannel>>;
