namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;

    using System;
    using System.Threading.Tasks;

    public class ImageInfoWriterService : IImageInfoWriterService
    {
        private readonly EverythingNBADbContext db;

        public ImageInfoWriterService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> WriteToDbAsync(string imageURL,/* string imageThumbnailURL,*/ string publicId, long imageLenght)
        {
            var image = new CloudinaryImage
            {
                ImageURL = imageURL,
                //ImageThumbnailURL = imageThumbnailURL,
                Length = imageLenght,
                ImagePublicId = publicId
            };

            await this.db.CloudinaryImages.AddAsync(image);

            await this.db.SaveChangesAsync();

            return image.Id;
        }
    }
}
