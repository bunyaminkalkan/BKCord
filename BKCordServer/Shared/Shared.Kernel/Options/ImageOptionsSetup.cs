using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Kernel.Options;
public sealed class ImageOptionsSetup : IConfigureOptions<ImageOptions>
{
    private const string ImageOptionsSection = nameof(ImageOptions);
    private readonly IConfiguration _configuration;

    public ImageOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ImageOptions options)
    {
        _configuration.GetSection(ImageOptionsSection).Bind(options);
    }
}