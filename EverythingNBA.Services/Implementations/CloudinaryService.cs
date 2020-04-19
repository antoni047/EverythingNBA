using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;

using EverythingNBA.Services.Utilities;

namespace EverythingNBA.Services.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {
        private Cloudinary cloudinary;
        private readonly IConfiguration configuration;

        public CloudinaryService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.StartCloudinary();
        }

        public async Task DeleteImages(params string[] publicIds)
        {
            var deleteParams = new DelResParams
            {
                PublicIds = publicIds.ToList(),
                Invalidate = true
            };

            await cloudinary.DeleteResourcesAsync(deleteParams);
        }

        public string GetImageURL(string publicId)
        {
            var imageURL = this.cloudinary.Api.UrlImgUp.BuildUrl(publicId);

            return imageURL;
        }

        public async Task<ImageUploadResult> UploadFormFileAsync(IFormFile file)
        {
            if (file == null) { return null; }

            using var memoryStream = file.OpenReadStream();

            var generator = new Random();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), memoryStream),
                PublicId = $"nba{generator.Next(0, 999999).ToString("000000")}{Path.GetFileNameWithoutExtension(file.FileName)}",
                Transformation = new Transformation().Crop("limit").Width(800).Height(600),
                EagerTransforms = new List<Transformation>()
                {
                    new Transformation().Width(200).Height(200).Crop("thumb")
                }
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return result;
        }

        private void StartCloudinary()
        {
            this.cloudinary = new Cloudinary(
                new Account(
                    configuration.GetSection("Cloudinary:AppName").Value,
                    configuration.GetSection("Cloudinary:ApiKey").Value,
                    configuration.GetSection("Cloudinary:ApiSecret").Value));
        }
    }
}
