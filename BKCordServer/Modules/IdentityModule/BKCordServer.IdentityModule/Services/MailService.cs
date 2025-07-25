
using FluentEmail.Core;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.Services;
internal sealed class MailService : IMailService
{
    private readonly IFluentEmail _fluentEmail;

    public MailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var sendResponse = await _fluentEmail.To(to).Subject(subject).Body(body).SendAsync(cancellationToken);

        if (!sendResponse.Successful)
            throw new InternalServerErrorException(string.Join(",", sendResponse.ErrorMessages));
    }
}
