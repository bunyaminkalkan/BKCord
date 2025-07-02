namespace Shared.Kernel.Options;
public class ImageOptions
{
    public string BasePath { get; set; } = string.Empty;
    public long MaxFileSize { get; set; }
    public List<string> AllowedExtensions { get; set; } = new List<string>();
}
