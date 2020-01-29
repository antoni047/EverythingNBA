using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EverythingNBA.Services
{
    public interface IImageService
    {
        Task<int> UploadImageAsync(IFormFile imageFile);

        Task DeleteImageAsync(int imageId);
    }
}
