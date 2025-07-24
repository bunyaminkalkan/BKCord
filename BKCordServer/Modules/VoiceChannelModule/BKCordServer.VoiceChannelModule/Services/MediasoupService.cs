using BKCordServer.VoiceChannelModule.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json; // JsonSerializer için eklendi

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

    /// <summary>
    /// Router'ın RTP yeteneklerini Mediasoup sunucusundan alır.
    /// İstemcinin odaya katılmadan önce alması gereken ilk bilgidir.
    /// </summary>
    /// <param name="roomId">Oda ID'si</param>
    /// <returns>Router RTP Yetenekleri nesnesi.</returns>
    public async Task<object?> GetRouterRtpCapabilitiesAsync(string roomId)
    {
        try
        {
            _log.LogInformation("Requesting Router RTP Capabilities for Room: {roomId}", roomId);
            var resp = await _http.GetAsync($"{_baseUrl}/rooms/{roomId}/router-rtp-capabilities");
            resp.EnsureSuccessStatusCode(); // Başarısız durum kodlarında hata fırlat
            var capabilities = await resp.Content.ReadFromJsonAsync<object>();
            _log.LogInformation("Successfully retrieved Router RTP Capabilities for Room: {roomId}", roomId);
            return capabilities;
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "GetRouterRtpCapabilities failed for RoomId: {roomId}. Error: {errorMessage}", roomId, ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize RouterRtpCapabilities response for RoomId: {roomId}. Error: {errorMessage}", roomId, ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Bir kullanıcıyı belirli bir odaya dahil eder.
    /// </summary>
    /// <param name="roomId">Katılınacak oda ID'si.</param>
    /// <param name="userId">Katılan kullanıcının ID'si.</param>
    /// <returns>Katılma yanıtı (başarı durumu ve diğer producer ID'leri).</returns>
    public async Task<JoinRoomResponse> JoinRoomAsync(string roomId, string userId)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/join
        var req = new { userId }; // roomId URL'de gönderiliyor, sadece userId body'de
        try
        {
            _log.LogInformation("Attempting to join Room: {roomId} with User: {userId}", roomId, userId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/join", req);
            resp.EnsureSuccessStatusCode(); // Başarısız durum kodlarında hata fırlat

            var joinResponse = await resp.Content.ReadFromJsonAsync<JoinRoomResponse>();
            if (joinResponse != null)
            {
                joinResponse.Success = true; // Set success based on HTTP status code
                joinResponse.RoomId = roomId; // Fill RoomId from request for consistency
                joinResponse.UserId = userId; // Fill UserId from request for consistency
                _log.LogInformation("User {userId} successfully joined Room: {roomId}. Found {producerCount} existing producers.", userId, roomId, joinResponse.ProducerIds?.Count ?? 0);
                return joinResponse;
            }
            return new JoinRoomResponse { Success = false, Message = "Failed to deserialize join room response." };
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "JoinRoom failed for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return new JoinRoomResponse { Success = false, Message = ex.Message, RoomId = roomId, UserId = userId };
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize join room response for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return new JoinRoomResponse { Success = false, Message = "Invalid response format.", RoomId = roomId, UserId = userId };
        }
    }

    /// <summary>
    /// Bir kullanıcıyı belirli bir odadan çıkarır.
    /// </summary>
    /// <param name="roomId">Çıkılacak oda ID'si.</param>
    /// <param name="userId">Çıkan kullanıcının ID'si.</param>
    public async Task LeaveRoomAsync(string roomId, string userId)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/leave
        var req = new { userId }; // roomId URL'de gönderiliyor, sadece userId body'de
        try
        {
            _log.LogInformation("Attempting to leave Room: {roomId} with User: {userId}", roomId, userId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/leave", req);
            resp.EnsureSuccessStatusCode();
            _log.LogInformation("User {userId} successfully left Room: {roomId}", userId, roomId);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "LeaveRoom failed for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
        }
    }

    /// <summary>
    /// Mediasoup sunucusunda yeni bir WebRTC Transport oluşturur.
    /// </summary>
    /// <param name="roomId">Odanın ID'si.</param>
    /// <param name="userId">Transport'u oluşturacak kullanıcının ID'si.</param>
    /// <returns>Oluşturulan Transport'un yanıtı.</returns>
    public async Task<TransportResponse?> CreateTransportAsync(string roomId, string userId)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/peers/:userId/transports
        // Mediasoup transport oluşturma isteğinde body boş olabilir, veya ek seçenekler gönderilebilir.
        var req = new { }; // Empty object for now as per Node.js server
        try
        {
            _log.LogInformation("Attempting to create Transport for Room: {roomId}, User: {userId}", roomId, userId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/peers/{userId}/transports", req);
            resp.EnsureSuccessStatusCode();

            var transportResponse = await resp.Content.ReadFromJsonAsync<TransportResponse>();
            _log.LogInformation("Successfully created Transport {transportId} for User: {userId} in Room: {roomId}", transportResponse?.Id, userId, roomId);
            return transportResponse;
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "CreateTransport failed for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize transport response for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Mediasoup Transport'u istemcinin DTLS parametreleriyle bağlar.
    /// </summary>
    /// <param name="roomId">Odanın ID'si.</param>
    /// <param name="userId">Transport'u bağlayacak kullanıcının ID'si.</param>
    /// <param name="transportId">Bağlanacak Transport'un ID'si.</param>
    /// <param name="dtlsParameters">DTLS parametreleri.</param>
    public async Task ConnectTransportAsync(string roomId, string userId, string transportId, object dtlsParameters)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/peers/:userId/transports/:transportId/connect
        var req = new { dtlsParameters };
        try
        {
            _log.LogInformation("Attempting to connect Transport {transportId} for User: {userId} in Room: {roomId}", transportId, userId, roomId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/peers/{userId}/transports/{transportId}/connect", req);
            resp.EnsureSuccessStatusCode();
            _log.LogInformation("Successfully connected Transport {transportId} for User: {userId} in Room: {roomId}", transportId, userId, roomId);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ConnectTransport failed for RoomId: {roomId}, UserId: {userId}, TransportId: {transportId}. Error: {errorMessage}", roomId, userId, transportId, ex.Message);
        }
    }

    /// <summary>
    /// Medya göndermek için bir Producer oluşturur.
    /// </summary>
    /// <param name="roomId">Odanın ID'si.</param>
    /// <param name="userId">Producer'ı oluşturacak kullanıcının ID'si.</param>
    /// <param name="transportId">Kullanılacak Transport'un ID'si.</param>
    /// <param name="rtpParameters">RTP parametreleri.</param>
    /// <param name="kind">Medya türü (audio/video).</param>
    /// <returns>Oluşturulan Producer'ın yanıtı.</returns>
    public async Task<ProducerResponse?> ProduceAsync(string roomId, string userId, string transportId, object rtpParameters, string kind)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/peers/:userId/transports/:transportId/produce
        var req = new { rtpParameters, kind };
        try
        {
            _log.LogInformation("Attempting to create Producer ({kind}) for User: {userId}, Transport: {transportId} in Room: {roomId}", kind, userId, transportId, roomId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/peers/{userId}/transports/{transportId}/produce", req);
            resp.EnsureSuccessStatusCode();

            var producerResponse = await resp.Content.ReadFromJsonAsync<ProducerResponse>();
            if (producerResponse != null)
            {
                producerResponse.Kind = kind; // Node.js'den dönmeyebilir, client'tan gelen bilgiyi ekleyelim
                producerResponse.UserId = userId; // Node.js'den dönmeyebilir, client'tan gelen bilgiyi ekleyelim
            }
            _log.LogInformation("Successfully created Producer {producerId} ({kind}) for User: {userId} in Room: {roomId}", producerResponse?.Id, kind, userId, roomId);
            return producerResponse;
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "Produce failed for RoomId: {roomId}, UserId: {userId}, TransportId: {transportId}. Error: {errorMessage}", roomId, userId, transportId, ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize producer response for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Medya almak için bir Consumer oluşturur.
    /// </summary>
    /// <param name="roomId">Odanın ID'si.</param>
    /// <param name="userId">Consumer'ı oluşturacak kullanıcının ID'si.</param>
    /// <param name="transportId">Kullanılacak Transport'un ID'si.</param>
    /// <param name="producerId">Tüketilecek Producer'ın ID'si.</param>
    /// <param name="rtpCapabilities">İstemcinin RTP yetenekleri.</param>
    /// <returns>Oluşturulan Consumer'ın yanıtı.</returns>
    public async Task<ConsumerResponse?> ConsumeAsync(string roomId, string userId, string transportId, string producerId, object rtpCapabilities)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/peers/:userId/transports/:transportId/consume
        var req = new { producerId, rtpCapabilities };
        try
        {
            _log.LogInformation("Attempting to create Consumer for User: {userId}, Transport: {transportId}, Producer: {producerId} in Room: {roomId}", userId, transportId, producerId, roomId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/peers/{userId}/transports/{transportId}/consume", req);
            resp.EnsureSuccessStatusCode();

            var consumerResponse = await resp.Content.ReadFromJsonAsync<ConsumerResponse>();
            _log.LogInformation("Successfully created Consumer {consumerId} for User: {userId}, Producer: {producerId} in Room: {roomId}", consumerResponse?.Id, userId, producerId, roomId);
            return consumerResponse;
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "Consume failed for RoomId: {roomId}, UserId: {userId}, TransportId: {transportId}, ProducerId: {producerId}. Error: {errorMessage}", roomId, userId, transportId, producerId, ex.Message);
            return null;
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize consumer response for RoomId: {roomId}, UserId: {userId}. Error: {errorMessage}", roomId, userId, ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Bir Consumer'ı devam ettirir (paused durumundan çıkarır).
    /// </summary>
    /// <param name="roomId">Odanın ID'si.</param>
    /// <param name="userId">Consumer'ın sahibi kullanıcının ID'si.</param>
    /// <param name="consumerId">Devam ettirilecek Consumer'ın ID'si.</param>
    public async Task ResumeConsumerAsync(string roomId, string userId, string consumerId)
    {
        // Node.js tarafında endpoint: POST /rooms/:roomId/peers/:userId/consumers/:consumerId/resume
        try
        {
            _log.LogInformation("Attempting to resume Consumer {consumerId} for User: {userId} in Room: {roomId}", consumerId, userId, roomId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/rooms/{roomId}/peers/{userId}/consumers/{consumerId}/resume", new { });
            resp.EnsureSuccessStatusCode();
            _log.LogInformation("Successfully resumed Consumer {consumerId} for User: {userId} in Room: {roomId}", consumerId, userId, roomId);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ResumeConsumer failed for RoomId: {roomId}, UserId: {userId}, ConsumerId: {consumerId}. Error: {errorMessage}", roomId, userId, consumerId, ex.Message);
        }
    }

    /// <summary>
    /// Bir kullanıcının sesini kapatıp açar (mute/unmute).
    /// NOT: Bu endpoint Node.js sunucunuzda şu anda mevcut değildir. Eklenmesi gereklidir.
    /// </summary>
    /// <param name="userId">Ses durumu değiştirilecek kullanıcının ID'si.</param>
    /// <returns>Mute işlemi sonucu.</returns>
    public async Task<MuteResult> ToggleMuteAsync(string userId)
    {
        // Node.js tarafında bu endpoint'in olması gerekiyor: POST /users/{userId}/toggle-mute
        // Veya daha iyisi: POST /rooms/{roomId}/peers/{userId}/toggle-mute
        // Mevcut Node.js server.js'de bu endpointler yok, bu yüzden bu çağrı başarısız olacaktır.
        try
        {
            _log.LogWarning("ToggleMuteAsync called. This endpoint might not be implemented on the Mediasoup server yet. User: {userId}", userId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/users/{userId}/toggle-mute", new { });
            resp.EnsureSuccessStatusCode();
            var muteResult = await resp.Content.ReadFromJsonAsync<MuteResult>();
            if (muteResult != null)
            {
                _log.LogInformation("User {userId} mute status toggled to: {isMuted}", userId, muteResult.IsMuted);
                return muteResult;
            }
            return new MuteResult { UserId = userId, IsMuted = false, Timestamp = DateTime.UtcNow }; // Varsayılan başarısızlık
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ToggleMute failed for UserId: {userId}. Error: {errorMessage}. (Is endpoint implemented on Node.js server?)", userId, ex.Message);
            return new MuteResult { UserId = userId, IsMuted = false, Timestamp = DateTime.UtcNow };
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize mute result for UserId: {userId}. Error: {errorMessage}", userId, ex.Message);
            return new MuteResult { UserId = userId, IsMuted = false, Timestamp = DateTime.UtcNow };
        }
    }

    /// <summary>
    /// Bir kullanıcının sesini kapatıp açar (deafen/undeafen).
    /// NOT: Bu endpoint Node.js sunucunuzda şu anda mevcut değildir. Eklenmesi gereklidir.
    /// </summary>
    /// <param name="userId">Ses durumu değiştirilecek kullanıcının ID'si.</param>
    /// <returns>Deafen işlemi sonucu.</returns>
    public async Task<DeafenResult> ToggleDeafenAsync(string userId)
    {
        // Node.js tarafında bu endpoint'in olması gerekiyor: POST /users/{userId}/toggle-deafen
        // Veya daha iyisi: POST /rooms/{roomId}/peers/{userId}/toggle-deafen
        // Mevcut Node.js server.js'de bu endpointler yok, bu yüzden bu çağrı başarısız olacaktır.
        try
        {
            _log.LogWarning("ToggleDeafenAsync called. This endpoint might not be implemented on the Mediasoup server yet. User: {userId}", userId);
            var resp = await _http.PostAsJsonAsync($"{_baseUrl}/users/{userId}/toggle-deafen", new { });
            resp.EnsureSuccessStatusCode();
            var deafenResult = await resp.Content.ReadFromJsonAsync<DeafenResult>();
            if (deafenResult != null)
            {
                _log.LogInformation("User {userId} deafen status toggled to: {isDeafened}", userId, deafenResult.IsDeafened);
                return deafenResult;
            }
            return new DeafenResult { UserId = userId, IsDeafened = false, Timestamp = DateTime.UtcNow }; // Varsayılan başarısızlık
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ToggleDeafen failed for UserId: {userId}. Error: {errorMessage}. (Is endpoint implemented on Node.js server?)", userId, ex.Message);
            return new DeafenResult { UserId = userId, IsDeafened = false, Timestamp = DateTime.UtcNow };
        }
        catch (JsonException ex)
        {
            _log.LogError(ex, "Failed to deserialize deafen result for UserId: {userId}. Error: {errorMessage}", userId, ex.Message);
            return new DeafenResult { UserId = userId, IsDeafened = false, Timestamp = DateTime.UtcNow };
        }
    }
}