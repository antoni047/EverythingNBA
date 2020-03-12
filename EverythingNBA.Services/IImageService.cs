namespace EverythingNBA.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<int> UploadImageAsync(IFormFile imageFile);

        Task DeleteImageAsync(int imageId);
    }
}
