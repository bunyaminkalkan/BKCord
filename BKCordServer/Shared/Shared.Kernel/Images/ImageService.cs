using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Options;

namespace Shared.Kernel.Images;

public class ImageService : IImageService
{
    private readonly ImageOptions _imageOptions;

    public ImageService(IOptions<ImageOptions> imageOptions)
    {
        _imageOptions = imageOptions.Value;
    }

    public async Task<string> UploadSingleImageAsync(IFormFile imageFile, string folder)
    {
        ValidateInputParameters(imageFile, folder);
        ValidateImageFile(imageFile);

        var uploadPath = CreateUploadPath(folder);
        var imageUrl = await UploadImageAsync(imageFile, folder, uploadPath);

        return imageUrl;
    }

    public void DeleteSingleImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        var fullPath = GetFullImagePath(imageUrl);
        TryDeleteFile(fullPath, imageUrl);
    }

    private static void ValidateInputParameters(IFormFile imageFile, string folder)
    {
        if (imageFile == null)
            throw new BadRequestException("Image file cannot be null.");

        if (string.IsNullOrWhiteSpace(folder))
            throw new BadRequestException("Folder name cannot be null or empty.");
    }

    private void ValidateImageFile(IFormFile imageFile)
    {
        ValidateContentType(imageFile);
        ValidateFileSize(imageFile);
    }

    private void ValidateContentType(IFormFile imageFile)
    {
        var contentType = imageFile.ContentType?.ToLowerInvariant();
        var allowedExtensions = _imageOptions.AllowedExtensions;

        if (string.IsNullOrEmpty(contentType) || !allowedExtensions.Contains(contentType))
        {
            var allowedTypes = string.Join(", ", allowedExtensions);
            throw new BadRequestException($"File type {contentType} is not allowed. Only {allowedTypes} are allowed.");
        }
    }

    private void ValidateFileSize(IFormFile imageFile)
    {
        if (imageFile.Length > _imageOptions.MaxFileSize)
        {
            var maxSizeMB = _imageOptions.MaxFileSize / (1024 * 1024);
            throw new BadRequestException($"File size of {imageFile.FileName} exceeds the maximum allowed size of {maxSizeMB} MB.");
        }

        if (imageFile.Length == 0)
        {
            throw new BadRequestException("File cannot be empty.");
        }
    }

    private string CreateUploadPath(string folder)
    {
        var uploadPath = Path.Combine(_imageOptions.BasePath, folder);
        EnsureDirectoryExists(uploadPath);
        return uploadPath;
    }

    private async Task<string> UploadImageAsync(IFormFile imageFile, string folder, string uploadPath)
    {
        var imageName = GenerateUniqueImageName(imageFile.ContentType);
        var filePath = Path.Combine(uploadPath, imageName);

        await SaveImageAsync(imageFile, filePath);

        return $"/uploads/{folder}/{imageName}";
    }

    private static string GenerateUniqueImageName(string contentType)
    {
        Dictionary<string, string> ContentTypeExtensions = new()
        {
            { "image/jpeg", ".jpg" },
            { "image/jpg", ".jpg" },
            { "image/png", ".png" },
            { "image/webp", ".webp" }
        };

        var extension = ContentTypeExtensions[contentType.ToLowerInvariant()];
        return $"{Guid.NewGuid()}{extension}";
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private static async Task SaveImageAsync(IFormFile imageFile, string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
    }

    private string GetFullImagePath(string imageUrl)
    {
        // URL formatını normalize et
        var normalizedUrl = imageUrl.Replace("/uploads", "").TrimStart('/');
        return Path.Combine(_imageOptions.BasePath, normalizedUrl);
    }

    private void TryDeleteFile(string fullPath, string imageUrl)
    {
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            throw new InternalServerErrorException($"Failed to delete image: {imageUrl}");
        }
    }
}