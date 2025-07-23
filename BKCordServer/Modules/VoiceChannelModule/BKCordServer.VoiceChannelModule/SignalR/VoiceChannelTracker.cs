using System.Collections.Concurrent;

namespace BKCordServer.VoiceChannelModule;

public static class VoiceChannelTracker
{
    public static ConcurrentDictionary<string, ChannelState> Connections = new();

    public static void Join(string connectionId, string channelId)
    {
        Connections[connectionId] = new ChannelState { ChannelId = channelId, IsMuted = false, IsDeafened = false };
    }

    public static void Leave(string connectionId)
    {
        Connections.TryRemove(connectionId, out _);
    }

    public static ChannelState Get(string connectionId)
    {
        return Connections.TryGetValue(connectionId, out var state) ? state : null;
    }
}

public class ChannelState
{
    public string ChannelId { get; set; }
    public bool IsMuted { get; set; }
    public bool IsDeafened { get; set; }
}