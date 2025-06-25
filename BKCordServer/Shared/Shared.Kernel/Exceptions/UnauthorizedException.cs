using System.Net;

namespace Shared.Kernel.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message)
        : base(message, (int)HttpStatusCode.Unauthorized) { }
}
