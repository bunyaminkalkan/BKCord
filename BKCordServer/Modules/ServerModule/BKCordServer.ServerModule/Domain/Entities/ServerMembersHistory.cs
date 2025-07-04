﻿namespace BKCordServer.ServerModule.Domain.Entities;

public class ServerMembersHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ServerId { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime LeftAt { get; set; }
    public string? RemovedReason { get; set; }
    public Guid? RemovedByUserId { get; set; }
    public bool WasBanned { get; set; } = false;
}
