using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Shared.Kernel.Services;
public sealed class HttpContextService : IHttpContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        return Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("user_id")?.Value!);
    }

    public Guid GetIdFromRoute(string key)
    {
        return Guid.Parse(_httpContextAccessor.HttpContext?.GetRouteValue(key)?.ToString()!);
    }
}
