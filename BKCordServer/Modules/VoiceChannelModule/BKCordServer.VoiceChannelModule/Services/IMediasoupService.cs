using BKCordServer.VoiceChannelModule.SignalR;

namespace BKCordServer.VoiceChannelModule.Services;

public interface IMediasoupService
{
    Task<JoinRoomResponse> JoinRoomAsync(string roomId, string userId);
    Task LeaveRoomAsync(string roomId, string userId);
    Task<TransportResponse> CreateTransportAsync(string roomId, string userId);
    Task<ProducerResponse> ProduceAsync(string transportId, object rtpParameters, string kind);
    Task<ConsumerResponse> ConsumeAsync(string roomId, string userId, string producerId, object rtpCapabilities);
    Task<MuteResult> ToggleMuteAsync(string userId);
    Task<DeafenResult> ToggleDeafenAsync(string userId);
}