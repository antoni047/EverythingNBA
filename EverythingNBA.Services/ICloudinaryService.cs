using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace EverythingNBA.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadFormFileAsync(IFormFile file);

        Task DeleteImages(params string[] publicIds);

        string GetImageURL(string publicId);

        //string GetImageThumbnailURL(string thumbnailPublicId);
    }
}
