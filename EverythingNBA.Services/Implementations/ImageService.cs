using System.Threading.Tasks;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

using EverythingNBA.Data;
using EverythingNBA.Models;

namespace EverythingNBA.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly ICloudinaryService cloudinary;
        private readonly EverythingNBADbContext db;
        private readonly IImageInfoWriterService imageInfoWriter;

        public ImageService(EverythingNBADbContext db, ICloudinaryService cloudinary, IImageInfoWriterService imageInfoWriter)
        {
            this.db = db;
            this.cloudinary = cloudinary;
            this.imageInfoWriter = imageInfoWriter;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var imageFromDb = await this.db.FindAsync<CloudinaryImage>(imageId);
            var imagePublicId = imageFromDb.ImagePublicId;

            this.db.CloudinaryImages.Remove(imageFromDb);
            await this.db.SaveChangesAsync();

            await this.cloudinary.DeleteImages(imagePublicId);
        }

        public async Task<int> UploadImageAsync(IFormFile imageFile)
        {
            var uploadResult = await this.cloudinary.UploadFormFileAsync(imageFile);

            var cloudinaryImageURL = this.cloudinary.GetImageURL(uploadResult.PublicId);

            var cloduinaryThumbnailImageURL = this.cloudinary.GetImageThumbnailURL(uploadResult.PublicId);

            var imageId = await this.imageInfoWriter.WriteToDbAsync(cloudinaryImageURL, cloduinaryThumbnailImageURL, uploadResult.PublicId, uploadResult.Length);

            return imageId;
        }
    }
}
