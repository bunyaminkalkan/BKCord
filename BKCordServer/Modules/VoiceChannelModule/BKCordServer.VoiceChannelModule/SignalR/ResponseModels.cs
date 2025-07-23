namespace BKCordServer.VoiceChannelModule.SignalR;
public class JoinRoomResponse
{
    public string RoomId { get; set; }
    public string UserId { get; set; }
    public object RouterRtpCapabilities { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}

public class TransportResponse
{
    public string Id { get; set; }
    public object IceParameters { get; set; }
    public object IceCandidates { get; set; }
    public object DtlsParameters { get; set; }
    public string Direction { get; set; } // "send" or "recv"
}

public class ProducerResponse
{
    public string Id { get; set; }
    public string Kind { get; set; }
    public string UserId { get; set; }
}

public class MuteResult
{
    public bool IsMuted { get; set; }
    public string UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class DeafenResult
{
    public bool IsDeafened { get; set; }
    public string UserId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ConsumerResponse
{
    public string Id { get; set; }
    public string ProducerId { get; set; }
    public string Kind { get; set; }
    public object RtpParameters { get; set; }
    public string Type { get; set; }
    public bool Paused { get; set; }
}