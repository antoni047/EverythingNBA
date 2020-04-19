namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadFormFileAsync(IFormFile file);

        Task DeleteImages(params string[] publicIds);

        string GetImageURL(string publicId);
    }
}
