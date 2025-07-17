using BKCordServer.VoiceChannelModule.Domain.Entities;
using MediatR;

namespace BKCordServer.VoiceChannelModule.UseCases.UpdateVoiceChannel;
public sealed record UpdateVoiceChannelCommand(string Name) : IRequest<VoiceChannel>;
