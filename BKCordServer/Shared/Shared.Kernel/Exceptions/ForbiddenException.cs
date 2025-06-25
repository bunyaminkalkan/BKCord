using System.Net;

namespace Shared.Kernel.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message)
        : base(message, (int)HttpStatusCode.Forbidden) { }
}
