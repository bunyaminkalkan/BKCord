using BKCordServer.VoiceChannelModule.SignalR;

namespace BKCordServer.VoiceChannelModule.Services;

public interface IMediasoupService
{
    Task<object?> GetRouterRtpCapabilitiesAsync(string roomId);
    Task<JoinRoomResponse> JoinRoomAsync(string roomId, string userId);
    Task LeaveRoomAsync(string roomId, string userId);
    Task<TransportResponse?> CreateTransportAsync(string roomId, string userId);
    Task ConnectTransportAsync(string roomId, string userId, string transportId, object dtlsParameters);
    Task<ProducerResponse?> ProduceAsync(string roomId, string userId, string transportId, object rtpParameters, string kind);
    Task<ConsumerResponse?> ConsumeAsync(string roomId, string userId, string transportId, string producerId, object rtpCapabilities);
    Task ResumeConsumerAsync(string roomId, string userId, string consumerId);
    Task<MuteResult> ToggleMuteAsync(string userId);
    Task<DeafenResult> ToggleDeafenAsync(string userId);
}