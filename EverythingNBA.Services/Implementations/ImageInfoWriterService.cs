namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Data;
    using EverythingNBA.Models;

    public class ImageInfoWriterService : IImageInfoWriterService
    {
        private readonly EverythingNBADbContext db;

        public ImageInfoWriterService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> WriteToDbAsync(string imageURL, string publicId, long imageLenght)
        {
            var image = new CloudinaryImage
            {
                ImageURL = imageURL,
                Length = imageLenght,
                ImagePublicId = publicId
            };

            await this.db.CloudinaryImages.AddAsync(image);

            await this.db.SaveChangesAsync();

            return image.Id;
        }
    }
}
