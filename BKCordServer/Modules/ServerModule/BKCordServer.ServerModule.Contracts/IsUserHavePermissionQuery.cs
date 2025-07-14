using MediatR;

namespace BKCordServer.ServerModule.Contracts;
public sealed record IsUserHavePermissionQuery(Guid UserId, Guid ServerId, RolePermission Permission) : IRequest<bool>;
