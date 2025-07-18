﻿using BKCordServer.ServerModule.Contracts;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.UpdateRole;
public sealed record UpdateRoleCommand(
    Guid RoleId,
    string Name,
    string Color,
    short Hierarchy,
    List<RolePermission> RolePermissions
    ) : IRequest<Domain.Entities.Role>;
