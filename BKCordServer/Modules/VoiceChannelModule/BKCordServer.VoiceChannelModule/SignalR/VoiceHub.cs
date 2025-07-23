using BKCordServer.VoiceChannelModule.Services;
using Microsoft.AspNetCore.SignalR;

namespace BKCordServer.VoiceChannelModule.SignalR;

public class VoiceHub : Hub
{
    private readonly IMediasoupService _mediasoup;

    public VoiceHub(IMediasoupService mediasoupService)
    {
        _mediasoup = mediasoupService;
    }

    // Join a voice channel group and notify mediasoup
    public async Task Join(string channelId)
    {
        var prev = VoiceChannelTracker.Get(Context.ConnectionId);
        if (prev != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, prev.ChannelId);
            await Clients.Group(prev.ChannelId).SendAsync("UserLeft", Context.ConnectionId);
            await _mediasoup.LeaveRoomAsync(prev.ChannelId, Context.ConnectionId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
        VoiceChannelTracker.Join(Context.ConnectionId, channelId);
        await _mediasoup.JoinRoomAsync(channelId, Context.ConnectionId);

        await Clients.Caller.SendAsync("Joined", channelId);
        await Clients.GroupExcept(channelId, Context.ConnectionId).SendAsync("UserJoined", Context.ConnectionId);
    }

    // Leave current channel
    public async Task Leave()
    {
        var state = VoiceChannelTracker.Get(Context.ConnectionId);
        if (state == null) return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, state.ChannelId);
        await Clients.Group(state.ChannelId).SendAsync("UserLeft", Context.ConnectionId);
        await _mediasoup.LeaveRoomAsync(state.ChannelId, Context.ConnectionId);

        VoiceChannelTracker.Leave(Context.ConnectionId);
        await Clients.Caller.SendAsync("Left");
    }

    // Toggle mute status
    public async Task ToggleMute()
    {
        var state = VoiceChannelTracker.Get(Context.ConnectionId);
        if (state == null) return;

        state.IsMuted = !state.IsMuted;
        await Clients.Caller.SendAsync("MuteChanged", state.IsMuted);
        await Clients.GroupExcept(state.ChannelId, Context.ConnectionId).SendAsync("UserMuteChanged", Context.ConnectionId, state.IsMuted);
    }

    // Toggle deafen status
    public async Task ToggleDeafen()
    {
        var state = VoiceChannelTracker.Get(Context.ConnectionId);
        if (state == null) return;

        state.IsDeafened = !state.IsDeafened;
        await Clients.Caller.SendAsync("DeafenChanged", state.IsDeafened);
        await Clients.GroupExcept(state.ChannelId, Context.ConnectionId).SendAsync("UserDeafenChanged", Context.ConnectionId, state.IsDeafened);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var state = VoiceChannelTracker.Get(Context.ConnectionId);
        if (state != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, state.ChannelId);
            await Clients.Group(state.ChannelId).SendAsync("UserDisconnected", Context.ConnectionId);
            await _mediasoup.LeaveRoomAsync(state.ChannelId, Context.ConnectionId);
            VoiceChannelTracker.Leave(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}