namespace Shared.Kernel.Services;
public interface IHttpContextService
{
    Guid GetUserId();
    Guid GetIdFromRoute(string key);
}