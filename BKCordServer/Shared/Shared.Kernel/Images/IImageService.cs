using Microsoft.AspNetCore.Http;

namespace Shared.Kernel.Images;
public interface IImageService
{
    Task<string> UploadSingleImageAsync(IFormFile imageFile, string folder);
    void DeleteSingleImage(string imageUrl);
}
