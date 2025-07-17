using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;

namespace BKCordServer.VoiceChannelModule.UseCases.CreateVoiceChannel;
public sealed record CreateVoiceChannelCommand(Guid ServerId, string Name) : IRequest<VoiceChannel>;
