namespace Shared.Kernel.Exceptions;

public class ErrorResult
{
    public required string Message { get; set; }
    public required int StatusCode { get; set; }
}
