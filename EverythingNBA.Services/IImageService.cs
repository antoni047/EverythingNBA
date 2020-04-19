namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;


    public interface IImageService
    {
        Task<int> UploadImageAsync(IFormFile imageFile);

        Task DeleteImageAsync(int imageId);
    }
}
