using System.Net;

namespace Shared.Kernel.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(message, (int)HttpStatusCode.NotFound) { }
}
