using System.Net;

namespace Shared.Kernel.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message)
        : base(message, (int)HttpStatusCode.BadRequest) { }
}
