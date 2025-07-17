using MediatR;

namespace BKCordServer.VoiceChannelModule.UseCases.DeleteVoiceChannel;
public sealed record DeleteVoiceChannelCommand(Guid VoiceChannelId) : IRequest;
