using BKCordServer.VoiceChannelModule.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BKCordServer.VoiceChannelModule.Services;

public class MediasoupService : IMediasoupService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly ILogger<MediasoupService> _log;

    public MediasoupService(HttpClient http, IConfiguration config, ILogger<MediasoupService> log)
    {
        _http = http;
        _baseUrl = config.GetConnectionString("MediasoupUrl") ?? "http://localhost:3000";
        _log = log;
    }

    public async Task<JoinRoomResponse> JoinRoomAsync(string roomId, string userId)
    {
        var req = new { roomId, userId };
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/join", req);
            return await resp.Content.ReadFromJsonAsync<JoinRoomResponse>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "JoinRoom failed");
            return new JoinRoomResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task LeaveRoomAsync(string roomId, string userId)
    {
        var req = new { roomId, userId };
        try
        {
            await _http.PostAsJsonAsync($"{_baseUrl}/rooms/leave", req);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "LeaveRoom failed");
        }
    }

    public async Task<TransportResponse> CreateTransportAsync(string roomId, string userId)
    {
        var req = new { roomId, userId };
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/transports/create", req);
            return await resp.Content.ReadFromJsonAsync<TransportResponse>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "CreateTransport failed");
            return null;
        }
    }

    public async Task ConnectTransportAsync(string transportId, object dtlsParameters)
    {
        var req = new { transportId, dtlsParameters };
        try
        {
            await _http.PostAsJsonAsync($"{_baseUrl}/transports/connect", req);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ConnectTransport failed");
        }
    }

    public async Task<ProducerResponse> ProduceAsync(string transportId, object rtpParameters, string kind)
    {
        var req = new { transportId, rtpParameters, kind };
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/producers/create", req);
            return await resp.Content.ReadFromJsonAsync<ProducerResponse>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "Produce failed");
            return null;
        }
    }

    public async Task<ConsumerResponse> ConsumeAsync(string roomId, string userId, string producerId, object rtpCapabilities)
    {
        var req = new { roomId, userId, producerId, rtpCapabilities };
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/consumers/create", req);
            return await resp.Content.ReadFromJsonAsync<ConsumerResponse>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "Consume failed");
            return null;
        }
    }

    public async Task<MuteResult> ToggleMuteAsync(string userId)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/users/{userId}/toggle-mute", new { });
            return await resp.Content.ReadFromJsonAsync<MuteResult>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ToggleMute failed");
            return new MuteResult { UserId = userId, IsMuted = false };
        }
    }

    public async Task<DeafenResult> ToggleDeafenAsync(string userId)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/users/{userId}/toggle-deafen", new { });
            return await resp.Content.ReadFromJsonAsync<DeafenResult>();
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ToggleDeafen failed");
            return new DeafenResult { UserId = userId, IsDeafened = false };
        }
    }
}