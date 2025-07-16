using MediatR;

namespace BKCordServer.ServerModule.Contracts;
public sealed record ValidateUserHavePermissionQuery(Guid UserId, Guid ServerId, RolePermission Permission) : IRequest;
