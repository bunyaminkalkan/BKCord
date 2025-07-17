using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;

namespace BKCordServer.VoiceChannelModule.UseCases.GetAllVoiceChannels;
public sealed record GetAllVoiceChannelsQuery(Guid ServerId) : IRequest<IEnumerable<VoiceChannel>>;

